namespace RJCP.Core.Environment.Native
{
    using System;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Xml;

    internal static class WinVersionFactory
    {
        private static readonly object s_Lock = new();
        private static NativeWinVersion s_Native;

        [SupportedOSPlatform("windows")]
        public static INativeWinVersion Create()
        {
            // This class has no state, so just return it.
            if (s_Native is null) {
                lock (s_Lock) {
                    s_Native ??= new();
                }
            }
            return s_Native;
        }

        /// <summary>
        /// Creates an instance of a <see cref="INativeWinVersion"/> for calculating Windows information.
        /// </summary>
        /// <param name="xmlFile">The XML file to load.</param>
        /// <returns>An instance of <see cref="INativeWinVersion"/> to query Windows version information.</returns>
        /// <exception cref="ArgumentNullException">File name <paramref name="xmlFile"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// File name <paramref name="xmlFile"/> is an invalid file name.
        /// <para>- or o</para>
        /// XML node 'WinVersionQuery/API' not found.
        /// </exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML file.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="xmlFile"/> specified a file that is read-only.
        /// <para>- or -</para>
        /// This operation is not supported on the current platform.
        /// <para>- or -</para>
        /// <paramref name="xmlFile"/> specified a directory.
        /// <para>- or -</para>
        /// The caller does not have the required permissions.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="xmlFile"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="xmlFile"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permissions.</exception>
        public static INativeWinVersion Create(string xmlFile)
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(xmlFile);

            XmlReaderSettings readSettings = new() {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null // Disable external entity resolution
            };

            using (var reader = XmlReader.Create(xmlFile, readSettings)) {
                XmlDocument winDoc = new() {
                    XmlResolver = null
                };
                winDoc.Load(reader);
                return Create(winDoc);
            }
        }

        /// <summary>
        /// Creates an instance of a <see cref="INativeWinVersion"/> for calculating Windows information.
        /// </summary>
        /// <param name="winDoc">The XML document to parse.</param>
        /// <returns>An instance of <see cref="INativeWinVersion"/> to query Windows version information.</returns>
        /// <exception cref="ArgumentNullException">Document <paramref name="winDoc"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">XML node 'WinVersionQuery/API' not found.</exception>
        /// <exception cref="System.Xml.XPath.XPathException">A prefix exists that's not defined is found.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML file.</exception>
        public static INativeWinVersion Create(XmlDocument winDoc)
        {
            ThrowHelper.ThrowIfNull(winDoc);

            XmlNode node = winDoc.SelectSingleNode("/WinVersionQuery") ??
                throw new ArgumentException("XML node '/WinVersionQuery' not found");
            return Create(node);
        }

        public static INativeWinVersion Create(XmlDocumentFragment winDocFragment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an instance of a <see cref="INativeWinVersion"/> for calculating Windows information.
        /// </summary>
        /// <param name="winDocNode">The XML node where to start parsing from.</param>
        /// <returns>An instance of <see cref="INativeWinVersion"/> to query Windows version information.</returns>
        /// <exception cref="ArgumentNullException">Document <paramref name="winDocNode"/> is <see langword="null"/>.</exception>
        public static INativeWinVersion Create(XmlNode winDocNode)
        {
            ThrowHelper.ThrowIfNull(winDocNode);

            return new XmlWinVersion(winDocNode);
        }
    }
}
