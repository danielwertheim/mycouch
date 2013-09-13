using System.Linq;
using System.Net.Http;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;
using Newtonsoft.Json;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class JsonSearchQueryResponseFactory : QueryResponseFactoryBase
    {
        protected IQueryResponseRowsDeserializer RowsDeserializer { get; set; }

        public JsonSearchQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            RowsDeserializer = new ViewQueryResponseRowsDeserializer(serializationConfiguration);
        }

        public virtual JsonSearchQueryResponse Create(HttpResponseMessage response)
        {
            return Materialize(new JsonSearchQueryResponse(), response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected override void OnPopulateRows<T>(QueryResponse<T> response, JsonReader jr)
        {
            response.Rows = RowsDeserializer.Deserialize<T>(jr).ToArray();
        }
    }
}