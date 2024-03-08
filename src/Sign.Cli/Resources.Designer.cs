﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sign.Cli {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sign.Cli.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application name (ClickOnce)..
        /// </summary>
        internal static string ApplicationNameOptionDescription {
            get {
                return ResourceManager.GetString("ApplicationNameOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Base directory for files.  Overrides the current working directory..
        /// </summary>
        internal static string BaseDirectoryOptionDescription {
            get {
                return ResourceManager.GetString("BaseDirectoryOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use Windows Certificate Store or a local certificate file..
        /// </summary>
        internal static string CertificateStoreCommandDescription {
            get {
                return ResourceManager.GetString("CertificateStoreCommandDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sign binaries and containers..
        /// </summary>
        internal static string CodeCommandDescription {
            get {
                return ResourceManager.GetString("CodeCommandDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Description of the signing certificate..
        /// </summary>
        internal static string DescriptionOptionDescription {
            get {
                return ResourceManager.GetString("DescriptionOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Description URL of the signing certificate..
        /// </summary>
        internal static string DescriptionUrlOptionDescription {
            get {
                return ResourceManager.GetString("DescriptionUrlOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digest algorithm to hash files with. Allowed values are &apos;Sha256&apos;, &apos;Sha384&apos;, and &apos;Sha512&apos;..
        /// </summary>
        internal static string FileDigestOptionDescription {
            get {
                return ResourceManager.GetString("FileDigestOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path to file containing paths of files to sign within an archive..
        /// </summary>
        internal static string FileListOptionDescription {
            get {
                return ResourceManager.GetString("FileListOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value for {0}. The value must be a fully rooted directory path..
        /// </summary>
        internal static string InvalidBaseDirectoryValue {
            get {
                return ResourceManager.GetString("InvalidBaseDirectoryValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value for {0}. The value must be &apos;Sha256&apos;, &apos;Sha384&apos;, or &apos;Sha512&apos;..
        /// </summary>
        internal static string InvalidDigestValue {
            get {
                return ResourceManager.GetString("InvalidDigestValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value for {0}. The value must be a number value greater than or equal to 1..
        /// </summary>
        internal static string InvalidMaxConcurrencyValue {
            get {
                return ResourceManager.GetString("InvalidMaxConcurrencyValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value for {0}. The value must be the certificate&apos;s SHA-1 thumbprint..
        /// </summary>
        internal static string InvalidSha1ThumbprintValue {
            get {
                return ResourceManager.GetString("InvalidSha1ThumbprintValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value for {0}. The value must be an absolute HTTP or HTTPS URL..
        /// </summary>
        internal static string InvalidUrlValue {
            get {
                return ResourceManager.GetString("InvalidUrlValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum concurrency..
        /// </summary>
        internal static string MaxConcurrencyOptionDescription {
            get {
                return ResourceManager.GetString("MaxConcurrencyOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Output file or directory. If omitted, input files will be overwritten..
        /// </summary>
        internal static string OutputOptionDescription {
            get {
                return ResourceManager.GetString("OutputOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Publisher name (ClickOnce)..
        /// </summary>
        internal static string PublisherNameOptionDescription {
            get {
                return ResourceManager.GetString("PublisherNameOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sign CLI.
        /// </summary>
        internal static string SignCommandDescription {
            get {
                return ResourceManager.GetString("SignCommandDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digest algorithm for the RFC 3161 timestamp server. Allowed values are Sha256, Sha384, and Sha512..
        /// </summary>
        internal static string TimestampDigestOptionDescription {
            get {
                return ResourceManager.GetString("TimestampDigestOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RFC 3161 timestamp server URL..
        /// </summary>
        internal static string TimestampUrlOptionDescription {
            get {
                return ResourceManager.GetString("TimestampUrlOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sets the verbosity level. Allowed values are &apos;none&apos;, &apos;critical&apos;, &apos;error&apos;, &apos;warning&apos;, &apos;information&apos;, &apos;debug&apos;, and &apos;trace&apos;..
        /// </summary>
        internal static string VerbosityOptionDescription {
            get {
                return ResourceManager.GetString("VerbosityOptionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only Windows x64 is supported at this time. See https://github.com/dotnet/sign/issues/474 regarding Windows x86 support..
        /// </summary>
        internal static string x86NotSupported {
            get {
                return ResourceManager.GetString("x86NotSupported", resourceCulture);
            }
        }
    }
}
