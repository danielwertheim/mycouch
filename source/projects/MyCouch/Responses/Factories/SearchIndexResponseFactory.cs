using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class SearchIndexResponseFactory : ResponseFactoryBase
    {
        protected readonly SearchIndexResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public SearchIndexResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new SearchIndexResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<SearchIndexResponse> CreateAsync(HttpResponseMessage httpResponse)
        {
            return await MaterializeAsync<SearchIndexResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }

        public virtual async Task<SearchIndexResponse<TIncludedDoc>> CreateAsync<TIncludedDoc>(HttpResponseMessage httpResponse)
        {
            return await MaterializeAsync<SearchIndexResponse<TIncludedDoc>>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync);
        }
    }
}