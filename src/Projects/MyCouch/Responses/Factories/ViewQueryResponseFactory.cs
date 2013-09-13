using System.Linq;
using System.Net.Http;
using MyCouch.Serialization;
using Newtonsoft.Json;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : QueryResponseFactoryBase
    {
        protected IQueryResponseRowsDeserializer RowsDeserializer { get; set; }

        public ViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            RowsDeserializer = new ViewQueryResponseRowsDeserializer(serializationConfiguration);
        }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ViewQueryResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected override void OnPopulateRows<T>(QueryResponse<T> response, JsonReader jr)
        {
            response.Rows = RowsDeserializer.Deserialize<T>(jr).ToArray();
        }
    }
}