namespace RJCP.Core.Environment.Native
{
    using System;

    internal interface IRegistryKey : IDisposable
    {
        object GetValue(string key);
    }
}
