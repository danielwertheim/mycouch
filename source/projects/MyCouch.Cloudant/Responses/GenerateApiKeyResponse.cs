using System;
using MyCouch.Responses;
using Newtonsoft.Json;

namespace MyCouch.Cloudant.Responses
{
#if !PCL
    [Serializable]
#endif
    public class GenerateApiKeyResponse : Response
    {
        [JsonProperty(JsonScheme.Key)]
        public string Key { get; set; }

        [JsonProperty(JsonScheme.Password)]
        public string Password { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}Key: {2}{0}Password: {3}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Key,
                Password);
        }
    }
}