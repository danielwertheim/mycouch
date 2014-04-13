using System.IO;
using System.Net.Http;
using System.Text;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories.Materializers
{
    public class DocumentResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public DocumentResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual void Materialize(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            SetContent(response, httpResponse);
        }

        protected virtual async void SetContent(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                if (ContentShouldContainIdAndRev(httpResponse.RequestMessage))
                    SetDocumentHeaderFromResponseStream(response, content);
                else
                {
                    SetMissingIdFromRequestUri(response, httpResponse);
                    SetMissingRevFromRequestHeaders(response, httpResponse);
                }

                content.Position = 0;

                var sb = new StringBuilder();

                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    while (!reader.EndOfStream)
                        sb.Append(reader.ReadLine());
                }

                response.Content = sb.ToString();

                sb.Clear();
            }
        }

        protected virtual bool ContentShouldContainIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void SetDocumentHeaderFromResponseStream(DocumentResponse response, Stream content)
        {
            Serializer.Populate(response, content);
        }

        protected virtual void SetMissingIdFromRequestUri(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id) && httpResponse.RequestMessage.Method != HttpMethod.Post)
                response.Id = httpResponse.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void SetMissingRevFromRequestHeaders(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }
    }
}