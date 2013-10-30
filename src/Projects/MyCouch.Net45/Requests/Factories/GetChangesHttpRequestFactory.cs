using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetChangesHttpRequestFactory : HttpRequestFactoryBase
    {
        public GetChangesHttpRequestFactory(IConnection connection) : base(connection) {}

        public virtual HttpRequest Create(GetChangesRequest request)
        {
            return CreateFor<GetChangesRequest>(HttpMethod.Get, GenerateRequestUrl(request));
        }

        protected virtual string GenerateRequestUrl(GetChangesRequest request)
        {
            return string.Format("{0}/_changes{1}",
                Connection.Address,
                GenerateQueryString(request));
        }

        protected virtual string GenerateQueryString(GetChangesRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrEmpty(p) ? string.Empty : string.Concat("?", p);
        }

        protected virtual string GenerateQueryStringParams(GetChangesRequest request)
        {
            return string.Join("&", ConvertRequestToJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        /// <summary>
        /// Returns all configured options of <see cref="GetChangesRequest"/> as key values.
        /// The values are formatted to JSON-compatible strings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> ConvertRequestToJsonCompatibleKeyValues(GetChangesRequest request)
        {
            var kvs = new Dictionary<string, string>();

            if(request.Feed.HasValue)
                kvs.Add(KeyNames.Feed, request.Feed.Value.AsString());

            if (HasValue(request.Since))
                kvs.Add(KeyNames.Since, request.Since);

            if (request.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, request.IncludeDocs.Value.ToString().ToLower());

            if (request.Descending.HasValue)
                kvs.Add(KeyNames.Descending, request.Descending.Value.ToString().ToLower());

            if (request.Limit.HasValue)
                kvs.Add(KeyNames.Limit, request.Limit.Value.ToString(MyCouchRuntime.NumberFormat));

            if (request.Heartbeat.HasValue)
                kvs.Add(KeyNames.HeartBeat, request.Heartbeat.Value.ToString(MyCouchRuntime.NumberFormat));

            if (request.Timeout.HasValue)
                kvs.Add(KeyNames.Timeout, request.Timeout.Value.ToString(MyCouchRuntime.NumberFormat));

            if (request.Filter != null)
                kvs.Add(KeyNames.Filter, request.Filter);

            if (request.Style.HasValue)
                kvs.Add(KeyNames.Style, request.Style.Value.AsString());

            return kvs;
        }

        /// <summary>
        /// Contains the string representation (Key) of
        /// individual options for <see cref="GetChangesRequest"/>.
        /// </summary>
        protected static class KeyNames
        {
            public const string Feed = "feed";
            public const string Since = "since";
            public const string IncludeDocs = "include_docs";
            public const string Descending = "descending";
            public const string Limit = "limit";
            public const string HeartBeat = "heartbeat";
            public const string Timeout = "timeout";
            public const string Filter = "filter";
            public const string Style = "style";
        }
    }
}