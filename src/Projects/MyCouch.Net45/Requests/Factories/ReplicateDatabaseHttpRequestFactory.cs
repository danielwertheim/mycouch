using System.Linq;
using System.Net.Http;
using System.Text;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;

namespace MyCouch.Requests.Factories
{
    public class ReplicateDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public ReplicateDatabaseHttpRequestFactory(IServerClientConnection connection) : base(connection)
        {
            RequestUrlGenerator = new AppendingRequestUrlGenerator(connection.Address);
        }

        public virtual HttpRequest Create(ReplicateDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var createHttpRequest = CreateFor<ReplicateDatabaseRequest>(HttpMethod.Post, GenerateRequestUrl(request));

            createHttpRequest.SetJsonContent(GenerateBody(request));

            return createHttpRequest;
        }

        protected virtual string GenerateRequestUrl(ReplicateDatabaseRequest request)
        {
            return RequestUrlGenerator.Generate("_replicate");
        }

        protected virtual string GenerateBody(ReplicateDatabaseRequest request)
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

            json.Append(JsonScheme.EndObject);

            return json.ToString();
        }
    }
}