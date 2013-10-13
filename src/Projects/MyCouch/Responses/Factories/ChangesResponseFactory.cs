using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;
using MyCouch.Serialization.Readers;
using Newtonsoft.Json;

namespace MyCouch.Responses.Factories
{
    public class ChangesResponseFactory : ResponseFactoryBase
    {
        protected readonly JsonSerializer Serializer;

        public ChangesResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = JsonSerializer.Create(SerializationConfiguration.Settings);
        }

        public virtual ChangesResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ChangesResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                using (var sr = new StreamReader(content))
                {
                    using (var jr = SerializationConfiguration.ApplyConfigToReader(new MyCouchJsonReader(sr)))
                    {
                        Serializer.Populate(jr, response);
                    }
                }
            }
        }
    }
}