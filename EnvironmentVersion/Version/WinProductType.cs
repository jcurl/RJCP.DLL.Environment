namespace RJCP.Core.Environment.Version
{
    /// <summary>
    /// The Windows Product Type.
    /// </summary>
    public enum WinProductType
    {
        /// <summary>
        /// Unknown Product Type.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// The operating system is Windows NT based, and is Windows 2000 or newer.
        /// </summary>
        Workstation = 1,

        /// <summary>
        /// The operating system is Windows NT based, Windows 2000 or newer, is a server and acting as a Domain
        /// Controller.
        /// </summary>
        DomainController = 2,

        /// <summary>
        /// The operating system is Windows NT based, Windows 2000 or newer, is a server.
        /// </summary>
        Server = 3,
    }
}
