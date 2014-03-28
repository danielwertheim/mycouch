using System;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public static class ClientExecuteExtensions
    {
        public static Task<TextResponse> PerformAsync(this IMyCouchClient client, HeadDatabaseRequest request)
        {
            return client.Database.HeadAsync(request);
        }

        public static Task<TextResponse> PerformAsync(this IMyCouchClient client, GetDatabaseRequest request)
        {
            return client.Database.GetAsync(request);
        }

        public static Task<TextResponse> PerformAsync(this IMyCouchClient client, PutDatabaseRequest request)
        {
            return client.Database.PutAsync(request);
        }

        public static Task<TextResponse> PerformAsync(this IMyCouchClient client, DeleteDatabaseRequest request)
        {
            return client.Database.DeleteAsync(request);
        }

        public static Task<TextResponse> PerformAsync(this IMyCouchClient client, CompactDatabaseRequest request)
        {
            return client.Database.CompactAsync(request);
        }

        public static Task<TextResponse> PerformAsync(this IMyCouchClient client, ViewCleanupRequest request)
        {
            return client.Database.ViewCleanup(request);
        }

        public static Task<ChangesResponse> PerformAsync(this IMyCouchClient client, GetChangesRequest request)
        {
            return client.Changes.GetAsync(request);
        }

        public static Task<ContinuousChangesResponse> PerformAsync(this IMyCouchClient client, GetChangesRequest request, Action<string> onRead, CancellationToken cancellationToken)
        {
            return client.Changes.GetAsync(request, onRead, cancellationToken);
        }

        public static IObservable<string> PerformAsync(this IMyCouchClient client, GetChangesRequest request, CancellationToken cancellationToken)
        {
            return client.Changes.GetAsync(request, cancellationToken);
        }

        public static Task<BulkResponse> PerformAsync(this IMyCouchClient client, BulkRequest request)
        {
            return client.Documents.BulkAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, CopyDocumentRequest request)
        {
            return client.Documents.CopyAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, ReplaceDocumentRequest request)
        {
            return client.Documents.ReplaceAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, HeadDocumentRequest request)
        {
            return client.Documents.HeadAsync(request);
        }

        public static Task<DocumentResponse> PerformAsync(this IMyCouchClient client, GetDocumentRequest request)
        {
            return client.Documents.GetAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, PostDocumentRequest request)
        {
            return client.Documents.PostAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, PutDocumentRequest request)
        {
            return client.Documents.PutAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, DeleteDocumentRequest request)
        {
            return client.Documents.DeleteAsync(request);
        }

        public static Task<AttachmentResponse> PerformAsync(this IMyCouchClient client, GetAttachmentRequest request)
        {
            return client.Attachments.GetAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, PutAttachmentRequest request)
        {
            return client.Attachments.PutAsync(request);
        }

        public static Task<DocumentHeaderResponse> PerformAsync(this IMyCouchClient client, DeleteAttachmentRequest request)
        {
            return client.Attachments.DeleteAsync(request);
        }

        public static Task<EntityResponse<T>> PerformAsync<T>(this IMyCouchClient client, GetEntityRequest request) where T : class
        {
            return client.Entities.GetAsync<T>(request);
        }
    }
}