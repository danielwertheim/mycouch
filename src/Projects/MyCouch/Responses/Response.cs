using System;
using System.Net;
using System.Net.Http;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public Uri RequestUri { get; set; }
        public HttpMethod RequestMethod { get; set; }
        public string Error { get; set; }
        public string Reason { get; set; }
        public bool IsSuccess
        {
            get { return (int)StatusCode >= 200 && (int)StatusCode < 300; }
        }

        public override string ToString()
        {
#if DEBUG
            return ToStringDebugVersion();
#else
            return base.ToString();
#endif
        }

        public virtual string ToStringDebugVersion()
        {
            return string.Format("RequestUri: {1}{0}RequestMethod: {2}{0}Status: {3}({4}){0}Error:{5}{0}Reason: {6}",
                Environment.NewLine,
                RequestUri,
                RequestMethod,
                StatusCode,
                (int)StatusCode,
                Error ?? "<NULL>",
                Reason ?? "<NULL>");
        }
    }
}