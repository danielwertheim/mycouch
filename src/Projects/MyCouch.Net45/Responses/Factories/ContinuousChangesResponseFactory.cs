using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ContinuousChangesResponseFactory : ResponseFactoryBase
    {
        public ContinuousChangesResponseFactory(ISerializer serializer)
            : base(serializer)
        {
        }

        public virtual ContinuousChangesResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ContinuousChangesResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(ContinuousChangesResponse response, HttpResponseMessage httpResponse)
        {
        }
    }
}