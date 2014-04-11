using System.Linq;
using System.Net.Http;
using System.Text;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;
using MyCouch.Serialization;

namespace MyCouch.Requests.Factories
{
    public class ReplicateDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public ReplicateDatabaseHttpRequestFactory(IServerClientConnection connection, ISerializer serializer) : base(connection)
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

            var json = new StringBuilder();

            json.Append(JsonScheme.StartObject);
            json.AppendFormat(JsonScheme.MemberStringValueFormat, "source", request.Source);
            json.Append(JsonScheme.MemberDelimiter);
            json.AppendFormat(JsonScheme.MemberStringValueFormat, "target", request.Target);

            if (request.DocIds != null && request.DocIds.Any())
            {
                var docIdsString = string.Join(",", request.DocIds
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => string.Format("\"{0}\"", id)));
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberArrayValueFormat, "doc_ids", docIdsString);
            }

            if (!string.IsNullOrWhiteSpace(request.Proxy))
            {
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberStringValueFormat, "proxy", request.Proxy);
            }

            if (request.CreateTarget.HasValue)
            {
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberValueFormat, "create_target", request.CreateTarget.Value.ToJsonString());
            }

            if (request.Continuous.HasValue)
            {
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberValueFormat, "continuous", request.Continuous.Value.ToJsonString());
            }

            if (request.Cancel.HasValue)
            {
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberValueFormat, "cancel", request.Cancel.Value.ToJsonString());
            }

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberStringValueFormat, "filter", request.Filter);
            }

            if (request.QueryParams != null && request.QueryParams.Any())
            {
                json.Append(JsonScheme.MemberDelimiter);
                json.AppendFormat(JsonScheme.MemberObjectValueFormat, "query_params", Serializer.Serialize(request.QueryParams));
            }

            json.Append(JsonScheme.EndObject);

            return json.ToString();
        }
    }
}