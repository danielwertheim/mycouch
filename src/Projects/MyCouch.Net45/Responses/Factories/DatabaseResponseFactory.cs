using System.IO;
using System.Net.Http;
using System.Text;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DatabaseResponseFactory : ResponseFactoryBase
    {
        public DatabaseResponseFactory(ISerializer serializer)
            : base(serializer)
        { }

        public virtual DatabaseResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new DatabaseResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual async void OnSuccessfulResponse(DatabaseResponse response, HttpResponseMessage httpResponse)
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