using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly QueryResponseMaterializer ResponseMaterializer;

        public ViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            ResponseMaterializer = new QueryResponseMaterializer(SerializationConfiguration);
        }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<ViewQueryResponse<T>>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(HttpResponseMessage response, ViewQueryResponse<T> result) where T : class
        {
            using(var content = response.Content.ReadAsStream())
                ResponseMaterializer.Populate(result, content);
        }
    }
}