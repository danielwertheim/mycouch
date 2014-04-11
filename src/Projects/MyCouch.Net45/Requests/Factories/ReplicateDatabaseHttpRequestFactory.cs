using System.Collections.Generic;
using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class ReplicateDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public ReplicateDatabaseHttpRequestFactory(IServerClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            RequestUrlGenerator = new AppendingRequestUrlGenerator(connection.Address);
            Serializer = serializer;
        }

        public virtual HttpRequest Create(ReplicateDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<ReplicateDatabaseRequest>(HttpMethod.Post, GenerateRequestUrl(request));

            httpRequest.SetJsonContent(GenerateRequestBody(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(ReplicateDatabaseRequest request)
        {
            return RequestUrlGenerator.Generate("_replicate");
        }

        protected virtual string GenerateRequestBody(ReplicateDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();
            Ensure.That(request.Source, "request.Source").IsNotNullOrWhiteSpace();
            Ensure.That(request.Target, "request.Target").IsNotNullOrWhiteSpace();

            var tmp = new Dictionary<string, object> { { "source", request.Source }, { "target", request.Target } };

            if (request.HasDocIds())
                tmp.Add("doc_ids", request.DocIds);

            if (!string.IsNullOrWhiteSpace(request.Proxy))
                tmp.Add("proxy", request.Proxy);

            if (request.CreateTarget.HasValue)
                tmp.Add("create_target", request.CreateTarget);

            if (request.Continuous.HasValue)
                tmp.Add("continuous", request.Continuous.Value);

            if (request.Cancel.HasValue)
                tmp.Add("cancel", request.Cancel.Value);

            if (!string.IsNullOrWhiteSpace(request.Filter))
                tmp.Add("filter", request.Filter);

            if (request.HasQueryParams())
                tmp.Add("query_params", request.QueryParams);

            return Serializer.Serialize(tmp);
        }
    }
}