using System.Linq;
using System.Net.Http;
using MyCouch.Serialization;
using Newtonsoft.Json;

namespace MyCouch.Responses.Factories
{
    public class JsonViewQueryResponseFactory : QueryResponseFactoryBase
    {
        protected IQueryResponseRowsDeserializer RowsDeserializer { get; set; }

        public JsonViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            RowsDeserializer = new ViewQueryResponseRowsDeserializer(serializationConfiguration);
        }

        public virtual JsonViewQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new JsonViewQueryResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected override void OnPopulateRows<T>(QueryResponse<T> response, JsonReader jr)
        {
            response.Rows = RowsDeserializer.Deserialize<T>(jr).ToArray();
        }
    }
}