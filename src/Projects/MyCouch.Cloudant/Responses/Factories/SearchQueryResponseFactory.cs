using System.Net.Http;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class SearchQueryResponseFactory : QueryResponseFactoryBase
    {
        public SearchQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
        }

        public virtual SearchQueryResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return Materialize(new SearchQueryResponse<T>(), response, OnSuccessfulResponse, OnFailedResponse);
        }
    }
}