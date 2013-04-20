using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using EnsureThat;
using MyCouch.Serialization;
using Newtonsoft.Json.Linq;

namespace MyCouch
{
    public class DbClientResponseFactory : IDbClientResponseFactory
    {
        protected readonly ISerializer Serializer;

        public DbClientResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual DatabaseResponse CreateDatabaseResponse(HttpResponseMessage response)
        {
            return CreateResponse<DatabaseResponse>(response, OnSuccessfulResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual DocumentResponse CreateDocumentResponse(HttpResponseMessage response)
        {
            return CreateResponse<DocumentResponse>(response, OnSuccessfulResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual EntityResponse<T> CreateEntityResponse<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<EntityResponse<T>>(response, OnSuccessfulResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual ViewQueryResponse CreateViewQueryResponse(HttpResponseMessage response)
        {
            return CreateResponse<ViewQueryResponse>(response, OnSuccessfulResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        public virtual ViewQueryResponse<T> CreateViewQueryResponse<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<ViewQueryResponse<T>>(response, OnSuccessfulResponseContentMaterializer, OnFailedResponseContentMaterializer);
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
            else if (!result.IsSuccess)
                onFailedResponseContentMaterializer(response, result);

            return result;
        }

        protected virtual void OnSuccessfulResponseContentMaterializer(HttpResponseMessage response, DatabaseResponse result)
        {
        }

        protected virtual void OnSuccessfulResponseContentMaterializer(HttpResponseMessage response, DocumentResponse result)
        {
            var content = response.Content.ReadAsStreamAsync().Result;

            OnSuccessfulResponseContentMaterializer(response.Headers, content, result);

            if (result.RequestMethod == HttpMethod.Get)
            {
                content.Position = 0;
                using (var reader = new StreamReader(content, Encoding.UTF8))
                {
                    result.Content = reader.ReadToEnd();
                }
            }
        }

        protected virtual void OnSuccessfulResponseContentMaterializer<T>(HttpResponseMessage response, EntityResponse<T> result) where T : class
        {
            var content = response.Content.ReadAsStreamAsync().Result;

            OnSuccessfulResponseContentMaterializer(response.Headers, content, result);

            if (result.RequestMethod == HttpMethod.Get)
            {
                content.Position = 0;
                result.Entity = Serializer.Deserialize<T>(content);
            }
        }

        protected virtual void OnSuccessfulResponseContentMaterializer<T>(HttpResponseHeaders headers, Stream content, T result) where T : SingleDocumentResponse
        {
            if (result.ContentShouldHaveIdAndRev())
            {
                var kv = Serializer.Deserialize<IDictionary<string, dynamic>>(content);
                if (kv.ContainsKey("id"))
                    result.Id = kv["id"];

                if (kv.ContainsKey("rev"))
                    result.Rev = kv["rev"];
            }

            if (result.RequestMethod == HttpMethod.Get)
            {
                if (result.RequestUri.Segments.Any())
                    result.Id = result.RequestUri.Segments.Last();

                var etag = headers.ETag;
                if (etag != null && etag.Tag != null)
                    result.Rev = etag.Tag.Substring(1, etag.Tag.Length - 2);
            }
        }

        //TODO: Make more effective
        protected virtual void OnSuccessfulResponseContentMaterializer<T>(HttpResponseMessage response, ViewQueryResponse<T> result) where T : class
        {
            var content = response.Content.ReadAsStreamAsync().Result;
            var kv = Serializer.Deserialize<IDictionary<string, JToken>>(content);

            if (kv.ContainsKey("total_rows"))
                result.TotalRows = kv["total_rows"].Value<long>();

            if (kv.ContainsKey("offset"))
                result.OffSet = kv["offset"].Value<long>();

            if (kv.ContainsKey("rows"))
            {
                var rows = (JArray)kv["rows"];

                if (result is ViewQueryResponse<string>)
                {
                    result.Rows = rows.Select(r => new ViewQueryResponse<T>.Row
                    {
                        Id = r["id"].Value<string>(),
                        Key = r["key"].Value<string>(),
                        Value = r["value"].Select(vr => vr.ToString() as T).ToArray()
                    }).ToArray();
                }
                else
                {
                    result.Rows = rows.Select(r => new ViewQueryResponse<T>.Row
                    {
                        Id = r["id"].Value<string>(),
                        Key = r["key"].Value<string>(),
                        Value = Serializer.Deserialize<T>(r["value"].Select(vr => vr.ToString())).ToArray()
                    }).ToArray();
                }
            }
        }
        
        protected virtual void OnFailedResponseContentMaterializer<T>(HttpResponseMessage response, T result) where T : Response
        {
            var kv = Serializer.Deserialize<IDictionary<string, dynamic>>(response.Content.ReadAsStreamAsync().Result);
            if (kv.ContainsKey("error"))
                result.Error = kv["error"];

            if (kv.ContainsKey("reason"))
                result.Reason = kv["reason"];
        }
    }
}