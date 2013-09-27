using System;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public enum ChangesFeed
    {
        Normal = 0,
        Longpoll = 1,
        Continuous = 2
    }
}