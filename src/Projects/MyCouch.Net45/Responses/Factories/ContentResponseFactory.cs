using System.IO;
using System.Net.Http;
using System.Text;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ContentResponseFactory : ResponseFactoryBase
    {
        public ContentResponseFactory(ISerializer serializer)
            : base(serializer)
        { }

        public virtual ContentResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ContentResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual async void OnSuccessfulResponse(ContentResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
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
    }
}