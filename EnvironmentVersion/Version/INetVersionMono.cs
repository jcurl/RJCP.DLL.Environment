namespace RJCP.Core.Environment.Version
{
    using System;

    /// <summary>
    /// Interface INetVersionMono for getting the version of .NET Mono installed.
    /// </summary>
    public interface INetVersionMono : INetVersion
    {
        /// <summary>
        /// Gets the path to the Mono binary.
        /// </summary>
        string MonoPath { get; }

        /// <summary>
        /// Gets the version of the MSCorLib.DLL file installed.
        /// </summary>
        Version MsCorLibVersion { get; }

        /// <summary>
        /// Gets the architecture for the Mono runtime.
        /// </summary>
        string Architecture { get; }
    }
}
