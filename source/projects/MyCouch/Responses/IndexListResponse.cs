using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class IndexListResponse : Response
    {
        public Index[] Indexes { get; set; }
        public long IndexCount { get { return IsEmpty ? 0 : Indexes.LongCount(); } }
        public bool IsEmpty
        {
            get { return Indexes == null || Indexes.Length == 0; }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}IndexCount: {3}{0}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                IsEmpty,
                IndexCount);
        }
#if !PCL && !vNext
        [Serializable]
#endif
        public class Index
        {
            [JsonProperty(JsonScheme.DesignDoc)]
            public string DesignDoc { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public Definition Def { get; set; }

#if !PCL && !vNext
            [Serializable]
#endif
            public class Definition
            {
                public SortableField[] Fields { get; set; }
            }
        }
    }
}
