using System.Threading.Tasks;
using MyCouch.Commands;
using MyCouch.Responses;

namespace MyCouch
{
    public static class ClientExecuteExtensions
    {
        public static Task<BulkResponse> ExecuteAsync(this IClient client, BulkCommand cmd)
        {
            return client.Documents.BulkAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, CopyDocumentCommand cmd)
        {
            return client.Documents.CopyAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, ReplaceDocumentCommand cmd)
        {
            return client.Documents.ReplaceAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DocumentExistsCommand cmd)
        {
            return client.Documents.ExistsAsync(cmd);
        }

        public static Task<DocumentResponse> ExecuteAsync(this IClient client, GetDocumentCommand cmd)
        {
            return client.Documents.GetAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PostDocumentCommand cmd)
        {
            return client.Documents.PostAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PutDocumentCommand cmd)
        {
            return client.Documents.PutAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DeleteDocumentCommand cmd)
        {
            return client.Documents.DeleteAsync(cmd);
        }

        public static Task<AttachmentResponse> ExecuteAsync(this IClient client, GetAttachmentCommand cmd)
        {
            return client.Attachments.GetAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, PutAttachmentCommand cmd)
        {
            return client.Attachments.PutAsync(cmd);
        }

        public static Task<DocumentHeaderResponse> ExecuteAsync(this IClient client, DeleteAttachmentCommand cmd)
        {
            return client.Attachments.DeleteAsync(cmd);
        }

        public static Task<EntityResponse<T>> ExecuteAsync<T>(this IClient client, GetEntityCommand cmd) where T : class
        {
            return client.Entities.GetAsync<T>(cmd);
        }
    }
}