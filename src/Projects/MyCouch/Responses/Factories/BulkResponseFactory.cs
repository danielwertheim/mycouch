using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;
using MyCouch.Serialization.Readers;
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

        public virtual BulkResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new BulkResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(BulkResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                using (var sr = new StreamReader(content))
                {
                    using (var jr = SerializationConfiguration.ApplyConfigToReader(new MyCouchJsonReader(sr)))
                    {
                        response.Rows = Serializer.Deserialize<BulkResponse.Row[]>(jr);
                    }
                }
            }
        }
    }
}