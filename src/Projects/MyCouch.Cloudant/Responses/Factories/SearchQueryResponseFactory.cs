using System.Linq;
using System.Net.Http;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;
using Newtonsoft.Json;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class SearchQueryResponseFactory : QueryResponseFactoryBase
    {
        protected IQueryResponseRowsDeserializer RowsDeserializer { get; set; }

        public SearchQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            RowsDeserializer = new ViewQueryResponseRowsDeserializer(serializationConfiguration);
        }

        public virtual SearchQueryResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return Materialize(new SearchQueryResponse<T>(), response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected override void OnPopulateRows<T>(QueryResponse<T> response, JsonReader jr)
        {
            response.Rows = RowsDeserializer.Deserialize<T>(jr).ToArray();
        }
    }
}