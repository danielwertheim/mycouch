using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using EnsureThat;

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

        public virtual JsonDocumentResponse CreateJsonDocumentResponse(HttpResponseMessage response)
        {
            return CreateResponse<JsonDocumentResponse>(response, OnSuccessfulJsonDocumentResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<EntityResponse<T>>(response, OnSuccessfulEntityResponseContentMaterializer, OnFailedEntityResponseContentMaterializer);
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
                AssignMissingIdFromRequest(response, result);
                AssignMissingRevFromRequest(response, result);

                return;
            }

            using (var content = response.Content.ReadAsStreamAsync().Result)
                Client.Serializer.PopulateDocumentHeaderResponse(result, content);
        }

        protected virtual void OnSuccessfulJsonDocumentResponseContentMaterializer(HttpResponseMessage response, JsonDocumentResponse result)
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
            {
                OnSuccessfulDocumentResponseContentMaterializer(response, content, result);

                if (result.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                    {
                        result.Content = reader.ReadToEnd();
                    }
                }
            }
        }

        protected virtual void OnSuccessfulEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
            {
                OnSuccessfulDocumentResponseContentMaterializer(response, content, result);

                if (result.RequestMethod == HttpMethod.Get)
                {
                    content.Position = 0;
                    result.Entity = Client.Serializer.Deserialize<T>(content);
                }
            }
        }

        protected virtual void OnSuccessfulDocumentResponseContentMaterializer<T>(HttpResponseMessage response, Stream content, T result) where T : DocumentResponse
        {
            if (result.ContentShouldHaveIdAndRev())
                Client.Serializer.PopulateDocumentResponse(result, content);

            if (result.RequestMethod == HttpMethod.Get)
            {
                AssignMissingIdFromRequest(response, result);
                AssignMissingRevFromRequest(response, result);
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

        protected virtual void OnFailedDocumentHeaderResponseContentMaterializer(HttpResponseMessage response, IDocumentHeaderResponse result)
        {
            OnFailedResponseContentMaterializer(response, result);

            AssignMissingIdFromRequest(response, result);
        }

        protected virtual void OnFailedEntityResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class 
        {
            OnFailedDocumentHeaderResponseContentMaterializer(response, result);
        }

        protected virtual void AssignMissingIdFromRequest(HttpResponseMessage response, IDocumentHeaderResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
                result.Id = response.RequestMessage.RequestUri.Segments.LastOrDefault();
        }

        protected virtual void AssignMissingRevFromRequest(HttpResponseMessage response, IDocumentHeaderResponse result)
        {
            if (string.IsNullOrWhiteSpace(result.Rev))
            {
                var etag = response.Headers.ETag;
                if (etag != null && etag.Tag != null)
                    result.Rev = etag.Tag.Substring(1, etag.Tag.Length - 2);
            }
        }
    }
}