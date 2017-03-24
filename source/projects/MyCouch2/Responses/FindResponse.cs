using MyCouch.Serialization.Converters;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyCouch.Responses
{
#if net45
    [Serializable]
#endif
    public class FindResponse : FindResponse<string> { }

#if net45
    [Serializable]
#endif
    public class FindResponse<TIncludedDoc> : Response
    {
        [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
        public TIncludedDoc[] Docs { get; set; }
        public long DocCount { get { return IsEmpty ? 0 : Docs.LongCount(); } }
        public bool IsEmpty
        {
            get { return Docs == null || Docs.Length == 0; }
        }
    }
}
