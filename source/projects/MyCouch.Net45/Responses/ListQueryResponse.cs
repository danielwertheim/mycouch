using System;
using System.Linq;
using MyCouch.Extensions;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class ListQueryResponse : Response
    {
        public string Etag { get; set; }
        public string Content { get; set; }
        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Content); }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}IsEmpty: {2}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                IsEmpty);
        }
    }
}