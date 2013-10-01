using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public static class ClientExecuteExtensions
    {
        public static Task<ChangesResponse> ExecuteAsync(this IClient client, GetChangesRequest request)
        {
            return client.Changes.GetAsync(request);
        }

        public static Task<BulkResponse> ExecuteAsync(this IClient client, BulkRequest request)
        {
            return client.Documents.BulkAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, CopyDocumentRequest request)
        {
            return client.Documents.CopyAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, ReplaceDocumentRequest request)
        {
            return client.Documents.ReplaceAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DocumentExistsRequest request)
        {
            return client.Documents.ExistsAsync(request);
        }

        public static Task<DocumentResponse> ExecuteAsync(this IClient client, GetDocumentRequest request)
        {
            return client.Documents.GetAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PostDocumentRequest request)
        {
            return client.Documents.PostAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PutDocumentRequest request)
        {
            return client.Documents.PutAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DeleteDocumentRequest request)
        {
            return client.Documents.DeleteAsync(request);
        }

        public static Task<AttachmentResponse> ExecuteAsync(this IClient client, GetAttachmentRequest request)
        {
            return client.Attachments.GetAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PutAttachmentRequest request)
        {
            return client.Attachments.PutAsync(request);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DeleteAttachmentRequest request)
        {
            return client.Attachments.DeleteAsync(request);
        }

        public static Task<EntityResponse<T>> ExecuteAsync<T>(this IClient client, GetEntityRequest request) where T : class
        {
            return client.Entities.GetAsync<T>(request);
        }
    }
}