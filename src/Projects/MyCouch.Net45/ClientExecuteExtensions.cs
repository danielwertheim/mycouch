using System;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public static class ClientExecuteExtensions
    {
        public static Task<ChangesResponse> PerformAsync(this IClient client, GetChangesRequest request)
        {
            return client.Changes.GetAsync(request);
        }

        public static Task<ContinuousChangesResponse> PerformAsync(this IClient client, GetChangesRequest request, Action<string> onRead, CancellationToken cancellationToken)
        {
            return client.Changes.GetAsync(request, onRead, cancellationToken);
        }

        public static Task<BulkResponse> PerformAsync(this IClient client, BulkRequest request)
        {
            return client.Documents.BulkAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, CopyDocumentRequest request)
        {
            return client.Documents.CopyAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, ReplaceDocumentRequest request)
        {
            return client.Documents.ReplaceAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, DocumentExistsRequest request)
        {
            return client.Documents.ExistsAsync(request);
        }

        public static Task<DocumentResponse> PerformAsync(this IClient client, GetDocumentRequest request)
        {
            return client.Documents.GetAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, PostDocumentRequest request)
        {
            return client.Documents.PostAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, PutDocumentRequest request)
        {
            return client.Documents.PutAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, DeleteDocumentRequest request)
        {
            return client.Documents.DeleteAsync(request);
        }

        public static Task<AttachmentResponse> PerformAsync(this IClient client, GetAttachmentRequest request)
        {
            return client.Attachments.GetAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, PutAttachmentRequest request)
        {
            return client.Attachments.PutAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IClient client, DeleteAttachmentRequest request)
        {
            return client.Attachments.DeleteAsync(request);
        }

        public static Task<EntityResponse<T>> PerformAsync<T>(this IClient client, GetEntityRequest request) where T : class
        {
            return client.Entities.GetAsync<T>(request);
        }
    }
}