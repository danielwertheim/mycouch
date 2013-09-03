using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class IndexQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly IndexQueryResponseMaterializer ResponseMaterializer;

        public IndexQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            ResponseMaterializer = new IndexQueryResponseMaterializer(SerializationConfiguration);
        }

        public virtual IndexQueryResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<IndexQueryResponse<T>>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(HttpResponseMessage response, IndexQueryResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.Populate(result, content);
        }
    }
}