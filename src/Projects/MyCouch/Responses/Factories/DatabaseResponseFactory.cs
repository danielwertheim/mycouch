using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DatabaseResponseFactory : ResponseFactoryBase
    {
        public DatabaseResponseFactory(ISerializer serializer)
            : base(serializer)
        { }

        public virtual DatabaseResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new DatabaseResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(DatabaseResponse result, HttpResponseMessage response) { }
    }
}