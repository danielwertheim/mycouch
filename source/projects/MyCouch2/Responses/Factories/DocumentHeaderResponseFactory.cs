using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : ResponseFactoryBase
    {
        protected readonly DocumentHeaderResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedDocumentHeaderResponseMaterializer FailedResponseMaterializer;

        public DocumentHeaderResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new DocumentHeaderResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedDocumentHeaderResponseMaterializer(serializer);
        }

        public virtual async Task<DocumentHeaderResponse> CreateAsync(HttpResponseMessage httpResponse)
        {
            return await MaterializeAsync<DocumentHeaderResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }
    }
}