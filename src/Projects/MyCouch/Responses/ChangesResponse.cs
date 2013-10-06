using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ChangesResponse : ChangesResponse<string> { }

#if !NETFX_CORE
    [Serializable]
#endif
    public class ChangesResponse<TIncludedDoc> : Response
    {
        public class Row
        {
            public string Id { get; set; }
            public long Seq { get; set; }
            public Change[] Changes { get; set; }
            public bool Deleted { get; set; }
            public TIncludedDoc IncludedDoc { get; set; }
        }
    }
}