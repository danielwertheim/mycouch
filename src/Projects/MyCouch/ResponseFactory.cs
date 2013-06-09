using System;
using System.IO;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class ResponseFactory : IResponseFactory
    {
        protected readonly IClient Client;

        public ResponseFactory(IClient client)
        {
            Ensure.That(client, "client").IsNotNull();

            Client = client;
        }

        public virtual DatabaseResponse CreateDatabaseResponse(HttpResponseMessage response)
        {
            return CreateResponse<DatabaseResponse>(response, OnSuccessfulDatabaseResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual BulkResponse CreateBulkResponse(HttpResponseMessage response)
        {
            return CreateResponse<BulkResponse>(response, OnSuccessfulBulkResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual DocumentHeaderResponse CreateDocumentHeaderResponse(HttpResponseMessage response)
        {
            return CreateResponse<DocumentHeaderResponse>(response, OnSuccessfulDocumentHeaderResponseContentMaterializer, OnFailedDocumentHeaderResponseContentMaterializer);
        }

        public virtual DocumentResponse CreateDocumentResponse(HttpResponseMessage response)
        {
            return CreateResponse<DocumentResponse>(response, OnSuccessfulDocumentResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<EntityResponse<T>>(response, OnSuccessfulEntityResponseContentMaterializer, OnFailedEntityResponseContentMaterializer);
        }

        public virtual AttachmentResponse CreateAttachmentResponse(HttpResponseMessage response)
        {
            return CreateResponse<AttachmentResponse>(response, OnSuccessfulAttachmentResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual JsonViewQueryResponse CreateJsonViewQueryResponse(HttpResponseMessage response)
        {
            return CreateResponse<JsonViewQueryResponse>(response, OnSuccessfulViewQueryResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual ViewQueryResponse<T> CreateViewQueryResponse<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<ViewQueryResponse<T>>(response, OnSuccessfulViewQueryResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual T CreateResponse<T>(HttpResponseMessage response, Action<HttpResponseMessage, T> onSuccessfulResponseContentMaterializer, Action<HttpResponseMessage, T> onFailedResponseContentMaterializer) where T : Response, new()
        {
            var result = new T
            {
                RequestUri = response.RequestMessage.RequestUri,
                StatusCode = response.StatusCode,
                RequestMethod = response.RequestMessage.Method
            };

            if (result.IsSuccess)
                onSuccessfulResponseContentMaterializer(response, result);
            else
                onFailedResponseContentMaterializer(response, result);

            return result;
        }

        protected virtual void OnSuccessfulDatabaseResponseContentMaterializer(HttpResponseMessage response, DatabaseResponse result) { }

        protected virtual void OnSuccessfulBulkResponseContentMaterializer(HttpResponseMessage response, BulkResponse result)
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
                Client.Serializer.PopulateBulkResponse(result, content);
        }

        protected virtual void OnSuccessfulDocumentHeaderResponseContentMaterializer(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (response.RequestMessage.Method == HttpMethod.Head)
            {
                AssignMissingIdFromRequestUri(response, result);
                AssignMissingRevFromRequestHeaders(response, result);

                return;
            }

            using (var content = response.Content.ReadAsStreamAsync().Result)
                Client.Serializer.PopulateDocumentHeaderResponse(result, content);
        }

        protected virtual void OnSuccessfulDocumentResponseContentMaterializer(HttpResponseMessage response, DocumentResponse result)
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    Client.Serializer.PopulateDocumentHeaderResponse(result, content);
                else
                {
                    AssignMissingIdFromRequestUri(response, result);
                    AssignMissingRevFromRequestHeaders(response, result);
                }

                content.Position = 0;
                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    result.Content = reader.ReadToEnd();
                }
            }
        }

        protected virtual void OnSuccessfulEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    Client.Serializer.PopulateDocumentHeaderResponse(result, content);
                else
                {
                    AssignMissingIdFromRequestUri(response, result);
                    AssignMissingRevFromRequestHeaders(response, result);
                }

                if (result.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    result.Entity = Client.Serializer.Deserialize<T>(content);
                }
            }
        }

        protected virtual void OnSuccessfulAttachmentResponseContentMaterializer(HttpResponseMessage response, AttachmentResponse result)
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
            {
                AssignMissingIdFromRequestUri(response, result);
                AssignMissingNameFromRequestUri(response, result);
                AssignMissingRevFromRequestHeaders(response, result);

                content.Position = 0;
                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    result.Content = Convert.FromBase64String(reader.ReadToEnd());
                }
            }
        }

        protected virtual void OnSuccessfulViewQueryResponseContentMaterializer<T>(HttpResponseMessage response, ViewQueryResponse<T> result) where T : class
        {
            using(var content = response.Content.ReadAsStreamAsync().Result)
                Client.Serializer.PopulateViewQueryResponse(result, content);
        }

        protected virtual void OnFailedResponseContentMaterializer<T>(HttpResponseMessage response, T result) where T : IResponse
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
                Client.Serializer.PopulateFailedResponse(result, content);
        }

        protected virtual void OnFailedDocumentHeaderResponseContentMaterializer(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            OnFailedResponseContentMaterializer(response, result);

            AssignMissingIdFromRequestUri(response, result);
        }

        protected virtual void OnFailedEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class 
        {
            OnFailedDocumentHeaderResponseContentMaterializer(response, result);
        }

        protected virtual bool ContentShouldHaveIdAndRev(HttpRequestMessage request)
        {
            return
                request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete;
        }

        protected virtual void AssignMissingIdFromRequestUri(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
                result.Id = response.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void AssignMissingIdFromRequestUri(HttpResponseMessage response, AttachmentResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
                result.Id = response.RequestMessage.GetUriSegmentByRightOffset(1);
        }

        protected virtual void AssignMissingNameFromRequestUri(HttpResponseMessage response, AttachmentResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Name))
                result.Name = response.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void AssignMissingRevFromRequestHeaders(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Rev))
                result.Rev = response.Headers.GetETag();
        }
    }
}