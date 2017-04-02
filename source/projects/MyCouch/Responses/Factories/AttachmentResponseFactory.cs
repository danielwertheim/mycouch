using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class AttachmentResponseFactory : ResponseFactoryBase
    {
        protected readonly AttachmentResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public AttachmentResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new AttachmentResponseMaterializer();
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<AttachmentResponse> CreateAsync(HttpResponseMessage httpResponse)
        {
            Ensure.That(httpResponse, "httpResponse").IsNotNull();

            return await MaterializeAsync<AttachmentResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }
    }
}