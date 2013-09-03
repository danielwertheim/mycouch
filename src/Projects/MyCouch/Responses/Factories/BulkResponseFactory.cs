using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;
using Newtonsoft.Json;

namespace MyCouch.Responses.Factories
{
    public class BulkResponseFactory : ResponseFactoryBase
    {
        protected readonly JsonSerializer Serializer;

        public BulkResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = JsonSerializer.Create(SerializationConfiguration.Settings);
        }

        public virtual BulkResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<BulkResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, BulkResponse result)
        {
            using (var content = response.Content.ReadAsStream())
            {
                using (var sr = new StreamReader(content))
                {
                    using (var jr = SerializationConfiguration.ReaderFactory(typeof(BulkResponse.Row[]), sr))
                    {
                        result.Rows = Serializer.Deserialize<BulkResponse.Row[]>(jr);
                    }
                }
            }
        }
    }
}