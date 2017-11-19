using System.Collections.Generic;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class ReplicateDatabaseServerHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public ReplicateDatabaseServerHttpRequestFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual HttpRequest Create(ReplicateDatabaseRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            return new HttpRequest(HttpMethod.Put, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(GenerateRequestBody(request));
        }

        protected virtual string GenerateRelativeUrl(ReplicateDatabaseRequest request)
        {
            return "/_replicator/" + new UrlSegment(request.Id);
        }

        protected virtual string GenerateRequestBody(ReplicateDatabaseRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));
            EnsureArg.IsNotNullOrWhiteSpace(request.Source, nameof(request.Source));
            EnsureArg.IsNotNullOrWhiteSpace(request.Target, nameof(request.Target));

            var tmp = new Dictionary<string, object>
            {
                { KeyNames.Source, request.Source },
                { KeyNames.Target, request.Target }
            };

            if (request.HasDocIds())
                tmp.Add(KeyNames.DocIds, request.DocIds);

            if (!string.IsNullOrWhiteSpace(request.Proxy))
                tmp.Add(KeyNames.Proxy, request.Proxy);

            if (request.CreateTarget.HasValue)
                tmp.Add(KeyNames.CreateTarget, request.CreateTarget);

            if (request.Continuous.HasValue)
                tmp.Add(KeyNames.Continuous, request.Continuous.Value);

            if (!string.IsNullOrWhiteSpace(request.Filter))
                tmp.Add(KeyNames.Filter, request.Filter);

            if (request.HasQueryParams())
                tmp.Add(KeyNames.QueryParams, request.QueryParams);

            return Serializer.Serialize(tmp);
        }

        protected static class KeyNames
        {
            public const string Source = "source";
            public const string Target = "target";
            public const string DocIds = "doc_ids";
            public const string Proxy = "proxy";
            public const string CreateTarget = "create_target";
            public const string Continuous = "continuous";
            public const string Cancel = "cancel";
            public const string Filter = "filter";
            public const string QueryParams = "query_params";
        }
    }
}