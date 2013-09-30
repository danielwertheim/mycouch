using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public static class ClientExecuteExtensions
    {
        public static Task<ChangesResponse> ExecuteAsync(this IClient client, GetChangesRequest cmd)
        {
            return client.Changes.GetAsync(cmd);
        }

        public static Task<BulkResponse> ExecuteAsync(this IClient client, BulkRequest cmd)
        {
            return client.Documents.BulkAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, CopyDocumentRequest cmd)
        {
            return client.Documents.CopyAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, ReplaceDocumentRequest cmd)
        {
            return client.Documents.ReplaceAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DocumentExistsRequest cmd)
        {
            return client.Documents.ExistsAsync(cmd);
        }

        public static Task<DocumentResponse> ExecuteAsync(this IClient client, GetDocumentRequest cmd)
        {
            return client.Documents.GetAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PostDocumentRequest cmd)
        {
            return client.Documents.PostAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PutDocumentRequest cmd)
        {
            return client.Documents.PutAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DeleteDocumentRequest cmd)
        {
            return client.Documents.DeleteAsync(cmd);
        }

        public static Task<AttachmentResponse> ExecuteAsync(this IClient client, GetAttachmentRequest cmd)
        {
            return client.Attachments.GetAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PutAttachmentRequest cmd)
        {
            return client.Attachments.PutAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DeleteAttachmentRequest cmd)
        {
            return client.Attachments.DeleteAsync(cmd);
        }

        public static Task<EntityResponse<T>> ExecuteAsync<T>(this IClient client, GetEntityRequest cmd) where T : class
        {
            return client.Entities.GetAsync<T>(cmd);
        }
    }
}