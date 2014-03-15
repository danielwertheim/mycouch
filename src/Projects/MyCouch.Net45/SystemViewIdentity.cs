using System;

namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain system view like all_docs.
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    public class SystemViewIdentity : ViewIdentity
    {
        public SystemViewIdentity(string name) : base(name) { }
    }
}