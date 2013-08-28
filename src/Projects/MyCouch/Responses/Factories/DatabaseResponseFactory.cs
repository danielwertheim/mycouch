using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DatabaseResponseFactory : ResponseFactoryBase
    {
        public DatabaseResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual DatabaseResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<DatabaseResponse>(response, OnSuccessfulDatabaseResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulDatabaseResponseContentMaterializer(HttpResponseMessage response, DatabaseResponse result) { }
    }
}