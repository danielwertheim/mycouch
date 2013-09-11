using System.Net.Http;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class IndexQueryResponseFactory : QueryResponseFactoryBase
    {
        public IndexQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
        }

        public virtual IndexQueryResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return Materialize(new IndexQueryResponse<T>(), response, OnSuccessfulResponse, OnFailedResponse);
        }
    }
}