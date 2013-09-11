using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : QueryResponseFactoryBase
    {
        public ViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
        }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage httpResponse) where T : class
        {
            return Materialize(new ViewQueryResponse<T>(), httpResponse, OnSuccessfulResponse, null);
        }
    }
}