﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE.txt file in the project root for more information.

using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using Sign.Core;

namespace Sign.Cli
{
    internal sealed class CertManagerCommand : Command
    {
        private readonly CodeCommand _codeCommand;

        internal Option<string?> SHA1ThumbprintOption { get; } = new(new[] { "-s", "--sha1" }, CertManagerResources.SHA1ThumbprintOptionDescription);
        internal Option<string?> CryptoServiceProvider { get; } = new(new[] { "-csp", "--crypto-service-provider" }, CertManagerResources.CSPOptionDescription);
        internal Option<string?> PrivateKeyContainer { get; } = new(new[] { "-k", "--key-container" }, CertManagerResources.KeyContainerOptionDescription);
        internal Option<string?> PrivateMachineKeyContainer { get; } = new(new[] { "-km", "--key-container-machine" }, CertManagerResources.MachineKeyContainerOptionDescription);

        internal Argument<string?> FileArgument { get; } = new("file(s)", AzureKeyVaultResources.FilesArgumentDescription);

        internal CertManagerCommand(CodeCommand codeCommand, IServiceProviderFactory serviceProviderFactory)
            : base("certificate-manager", Resources.LocalCommandDescription)
        {
            ArgumentNullException.ThrowIfNull(codeCommand, nameof(codeCommand));
            ArgumentNullException.ThrowIfNull(serviceProviderFactory, nameof(serviceProviderFactory));

            _codeCommand = codeCommand;

            SHA1ThumbprintOption.IsRequired = true;

            AddOption(SHA1ThumbprintOption);
            AddOption(CryptoServiceProvider);
            AddOption(PrivateKeyContainer);
            AddOption(PrivateMachineKeyContainer);
            AddArgument(FileArgument);

            this.SetHandler(async (InvocationContext context) =>
            {
                DirectoryInfo baseDirectory = context.ParseResult.GetValueForOption(_codeCommand.BaseDirectoryOption)!;
                string? applicationName = context.ParseResult.GetValueForOption(_codeCommand.ApplicationNameOption);
                string? publisherName = context.ParseResult.GetValueForOption(_codeCommand.PublisherNameOption);
                string? description = context.ParseResult.GetValueForOption(_codeCommand.DescriptionOption);
                // This option is required.  If its value fails to parse we won't have reached here,
                // and after successful parsing its value will never be null.
                // Non-null is already guaranteed; the null-forgiving operator (!) just simplifies code.
                Uri descriptionUrl = context.ParseResult.GetValueForOption(_codeCommand.DescriptionUrlOption)!;
                string? fileListFilePath = context.ParseResult.GetValueForOption(_codeCommand.FileListOption);
                HashAlgorithmName fileHashAlgorithmName = context.ParseResult.GetValueForOption(_codeCommand.FileDigestOption);
                HashAlgorithmName timestampHashAlgorithmName = context.ParseResult.GetValueForOption(_codeCommand.TimestampDigestOption);
                // This option is optional but has a default value.  If its value fails to parse we won't have
                // reached here, and after successful parsing its value will never be null.
                // Non-null is already guaranteed; the null-forgiving operator (!) just simplifies code.
                Uri timestampUrl = context.ParseResult.GetValueForOption(_codeCommand.TimestampUrlOption)!;
                LogLevel verbosity = context.ParseResult.GetValueForOption(_codeCommand.VerbosityOption);
                string? output = context.ParseResult.GetValueForOption(_codeCommand.OutputOption);
                int maxConcurrency = context.ParseResult.GetValueForOption(_codeCommand.MaxConcurrencyOption);

                string? sha1Thumbprint = context.ParseResult.GetValueForOption(SHA1ThumbprintOption);
                string? cryptoServiceProvider = context.ParseResult.GetValueForOption(CryptoServiceProvider);
                string? privateKeyContainer = context.ParseResult.GetValueForOption(PrivateKeyContainer);
                string? privateMachineKeyContainer = context.ParseResult.GetValueForOption(PrivateMachineKeyContainer);

                string? fileArgument = context.ParseResult.GetValueForArgument(FileArgument);

                if (string.IsNullOrEmpty(fileArgument))
                {
                    context.Console.Error.WriteLine(AzureKeyVaultResources.MissingFileValue);
                    context.ExitCode = ExitCode.InvalidOptions;
                    return;
                }

                // CSP requires either K or KM options but not both.
                if (!string.IsNullOrEmpty(cryptoServiceProvider)
                    && string.IsNullOrEmpty(privateKeyContainer) == string.IsNullOrEmpty(privateMachineKeyContainer))
                {
                    if (string.IsNullOrEmpty(privateKeyContainer) && string.IsNullOrEmpty(privateMachineKeyContainer))
                    {
                        // Both were empty and one is required.
                        context.Console.Error.WriteLine(CertManagerResources.MultiplePrivateKeyContainersError);
                        context.ExitCode = ExitCode.InvalidOptions;
                        return;
                    }
                    else
                    {
                        // Both were provided but can only use one.
                        context.Console.Error.WriteLine(CertManagerResources.NoPrivateKeyContainerError);
                        context.ExitCode = ExitCode.InvalidOptions;
                        return;
                    }
                }

                // Make sure this is rooted
                if (!Path.IsPathRooted(baseDirectory.FullName))
                {
                    context.Console.Error.WriteLine(
                        FormatMessage(
                            AzureKeyVaultResources.InvalidBaseDirectoryValue,
                            _codeCommand.BaseDirectoryOption));
                    context.ExitCode = ExitCode.InvalidOptions;
                    return;
                }

                IServiceProvider serviceProvider = serviceProviderFactory.Create(verbosity);

                List<FileInfo> inputFiles;

                // If we're going to glob, we can't be fully rooted currently (fix me later)

                bool isGlob = fileArgument.Contains('*');

                if (isGlob)
                {
                    if (Path.IsPathRooted(fileArgument))
                    {
                        context.Console.Error.WriteLine(AzureKeyVaultResources.InvalidFileValue);
                        context.ExitCode = ExitCode.InvalidOptions;

                        return;
                    }

                    IFileListReader fileListReader = serviceProvider.GetRequiredService<IFileListReader>();
                    IFileMatcher fileMatcher = serviceProvider.GetRequiredService<IFileMatcher>();

                    using (MemoryStream stream = new(Encoding.UTF8.GetBytes(fileArgument)))
                    using (StreamReader reader = new(stream))
                    {
                        fileListReader.Read(reader, out Matcher? matcher, out Matcher? antiMatcher);

                        DirectoryInfoBase directory = new DirectoryInfoWrapper(baseDirectory);

                        IEnumerable<FileInfo> matches = fileMatcher.EnumerateMatches(directory, matcher);

                        if (antiMatcher is not null)
                        {
                            IEnumerable<FileInfo> antiMatches = fileMatcher.EnumerateMatches(directory, antiMatcher);
                            matches = matches.Except(antiMatches, FileInfoComparer.Instance);
                        }

                        inputFiles = matches.ToList();
                    }
                }
                else
                {
                    inputFiles = new List<FileInfo>()
                    {
                        new FileInfo(ExpandFilePath(baseDirectory, fileArgument))
                    };
                }

                FileInfo? fileList = null;
                if (!string.IsNullOrEmpty(fileListFilePath))
                {
                    if (Path.IsPathRooted(fileListFilePath))
                    {
                        fileList = new FileInfo(fileListFilePath);
                    }
                    else
                    {
                        fileList = new FileInfo(ExpandFilePath(baseDirectory, fileListFilePath));
                    }
                }

                if (inputFiles.Count == 0)
                {
                    context.Console.Error.WriteLine(AzureKeyVaultResources.NoFilesToSign);
                    context.ExitCode = ExitCode.NoInputsFound;
                    return;
                }

                if (inputFiles.Any(file => !file.Exists))
                {
                    context.Console.Error.WriteLine(
                        FormatMessage(
                            AzureKeyVaultResources.SomeFilesDoNotExist,
                            _codeCommand.BaseDirectoryOption));

                    foreach (FileInfo file in inputFiles.Where(file => !file.Exists))
                    {
                        context.Console.Error.WriteLine($"    {file.FullName}");
                    }

                    context.ExitCode = ExitCode.NoInputsFound;
                    return;
                }

                if (string.IsNullOrEmpty(context.ParseResult.GetValueForOption(SHA1ThumbprintOption)))
                {
                    context.Console.Error.WriteLine(
                        FormatMessage(Resources.InvalidSha1ThumbrpintValue, SHA1ThumbprintOption));
                    context.ExitCode = ExitCode.NoInputsFound;

                    return;
                }

                ISigner signer = serviceProvider.GetRequiredService<ISigner>();

                context.ExitCode = await signer.SignAsync(
                    inputFiles,
                    output,
                    fileList,
                    baseDirectory,
                    applicationName,
                    publisherName,
                    description,
                    descriptionUrl,
                    timestampUrl,
                    maxConcurrency,
                    fileHashAlgorithmName,
                    timestampHashAlgorithmName,
                    tokenCredential: null,
                    keyVaultUrl: null,
                    certificateName: null,
                    SHA1Thumbprint: sha1Thumbprint!,
                    cryptoServiceProvider,
                    privateKeyContainer);
            });
        }

        private static string ExpandFilePath(DirectoryInfo baseDirectory, string file)
        {
            if (Path.IsPathRooted(file))
            {
                return file;
            }

            return Path.Combine(baseDirectory.FullName, file);
        }

        private static string FormatMessage(string format, params IdentifierSymbol[] symbols)
        {
            string[] formattedSymbols = symbols
                .Select(symbol => $"--{symbol.Name}")
                .ToArray();

            return string.Format(CultureInfo.CurrentCulture, format, formattedSymbols);
        }
    }
}