using System;

namespace MyCouch
{
    /// <summary>
    /// Connects to a server instance rather than a DB instance
    /// as <see cref="IMyCouchClient"/> or <see cref="IMyCouchStore"/>
    /// does.
    /// </summary>
    public interface IMyCouchServerClient : IDisposable { }
}