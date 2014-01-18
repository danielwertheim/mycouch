using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ChangesResponseFactory : ResponseFactoryBase
    {
        public ChangesResponseFactory(ISerializer serializer)
            : base(serializer)
        {
        }

        public virtual ChangesResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ChangesResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected async virtual void OnSuccessfulResponse<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}