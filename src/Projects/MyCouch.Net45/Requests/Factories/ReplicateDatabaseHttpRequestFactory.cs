using System.Net.Http;
using System.Text;
using EnsureThat;
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
            json.Append(JsonScheme.EndObject);

            return json.ToString();
        }
    }
}