using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories.Materializers
{
    public class FailedResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public FailedResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual void Materialize(Response response, HttpResponseMessage httpResponse)
        {
            var errorAndReasonExists = !string.IsNullOrWhiteSpace(response.Error) && !string.IsNullOrWhiteSpace(response.Reason);
            if (errorAndReasonExists)
                return;

            SetErrorAndReason(response, httpResponse);
        }

        protected virtual async void SetErrorAndReason(Response response, HttpResponseMessage httpResponse)
        {
            var info = await GetInfoAsync(httpResponse);

            if (string.IsNullOrWhiteSpace(response.Error))
                response.Error = info.Error;

            if (string.IsNullOrWhiteSpace(response.Reason))
                response.Reason = info.Reason;
        }

        protected virtual async Task<FailedResponseInfo> GetInfoAsync(HttpResponseMessage httpResponse)
        {
            var info = new FailedResponseInfo();

            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                Serializer.Populate(info, content);

            return info;
        }

#if !NETFX_CORE
        [Serializable]
#endif
        protected class FailedResponseInfo
        {
            public string Error { get; set; }
            public string Reason { get; set; }
        }
    }
}