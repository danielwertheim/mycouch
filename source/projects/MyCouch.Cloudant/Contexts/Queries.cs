﻿using MyCouch.EnsureThat;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.Cloudant.Contexts
{
    public class Queries : ApiContextBase<IDbConnection>, IQueries
    {
        protected PostIndexHttpRequestFactory PostIndexHttpRequestFactory { get; set; }
        protected GetAllIndexesHttpRequestFactory GetAllIndexesHttpRequestFactory { get; set; }
        protected DeleteIndexHttpRequestFactory DeleteIndexHttpRequestFactory { get; set; }
        protected FindHttpRequestFactory FindHttpRequestFactory { get; set; }
        protected IndexResponseFactory IndexResponseFactory { get; set; }
        protected IndexListResponseFactory IndexListResponseFactory { get; set; }
        protected FindResponseFactory FindResponseFactory { get; set; }

        public Queries(IDbConnection connection, ISerializer documentSerializer, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(documentSerializer, "documentSerializer").IsNotNull();
            Ensure.That(serializer, "serializer").IsNotNull();

            PostIndexHttpRequestFactory = new PostIndexHttpRequestFactory(serializer);
            GetAllIndexesHttpRequestFactory = new GetAllIndexesHttpRequestFactory();
            DeleteIndexHttpRequestFactory = new DeleteIndexHttpRequestFactory();
            FindHttpRequestFactory = new FindHttpRequestFactory(serializer);

            IndexResponseFactory = new IndexResponseFactory(serializer);
            IndexListResponseFactory = new IndexListResponseFactory(serializer);
            FindResponseFactory = new FindResponseFactory(documentSerializer);
        }

        public virtual async Task<IndexResponse> PostAsync(PostIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessIndexResponse(res);
            }
        }

        public virtual async Task<IndexListResponse> GetAllAsync()
        {
            var httpRequest = CreateHttpRequest();

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessIndexListResponse(res);
            }
        }

        public virtual async Task<IndexResponse> DeleteAsync(DeleteIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessIndexResponse(res);
            }
        }

        public virtual async Task<FindResponse> FindAsync(FindRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessFindResponse(res);
            }
        }

        public virtual async Task<FindResponse<TIncludedDoc>> FindAsync<TIncludedDoc>(FindRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessFindResponse<TIncludedDoc>(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteIndexRequest request)
        {
            return DeleteIndexHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest()
        {
            return GetAllIndexesHttpRequestFactory.Create();
        }

        protected virtual HttpRequest CreateHttpRequest(PostIndexRequest request)
        {
            return PostIndexHttpRequestFactory.Create(request);
        }

        protected virtual IndexResponse ProcessIndexResponse(HttpResponseMessage response)
        {
            return IndexResponseFactory.Create(response);
        }

        protected virtual IndexListResponse ProcessIndexListResponse(HttpResponseMessage response)
        {
            return IndexListResponseFactory.Create(response);
        }

        protected virtual HttpRequest CreateHttpRequest(FindRequest request)
        {
            return FindHttpRequestFactory.Create(request);
        }

        protected virtual FindResponse ProcessFindResponse(HttpResponseMessage response)
        {
            return FindResponseFactory.Create(response);
        }

        protected virtual FindResponse<TIncludedDoc> ProcessFindResponse<TIncludedDoc>(HttpResponseMessage response)
        {
            return FindResponseFactory.Create<TIncludedDoc>(response);
        }
    }
}
