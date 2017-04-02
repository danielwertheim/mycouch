using EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;
using System.Linq;
using System.Net.Http;
using System.Text;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class FindHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public FindHttpRequestFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "Serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual HttpRequest Create(FindRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(GenerateRequestBody(request));
        }

        protected virtual string GenerateRelativeUrl(FindRequest request)
        {
            return "/_find";
        }

        protected virtual string GenerateRequestBody(FindRequest request)
        {
            Ensure.That(request.SelectorExpression, "request.SelectorExpression").IsNotNullOrWhiteSpace();

            var sb = new StringBuilder();
            sb.Append("{");

            sb.AppendFormat(FormatStrings.JsonPropertyFormat, KeyNames.Selector, request.SelectorExpression);
            if(request.Limit.HasValue)
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Limit, Serializer.ToJson(request.Limit.Value));
            if (request.Skip.HasValue)
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Skip, Serializer.ToJson(request.Skip.Value));
            if(request.HasSortings())
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Sort, Serializer.ToJsonArray(request.Sort));
            if (request.HasFields())
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Fields, Serializer.ToJsonArray(request.Fields.ToArray()));
            if(request.ReadQuorum.HasValue)
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.ReadQuorum, Serializer.ToJson(request.ReadQuorum.Value));

            sb.Append("}");

            return sb.ToString();
        }

        protected virtual string GetSelectorContent(string selectorExpression)
        {
            var selector = Serializer.Deserialize<dynamic>(selectorExpression);
            return Serializer.ToJson(selector);
        }

        protected static class KeyNames
        {
            public const string Selector = "selector";
            public const string Limit = "limit";
            public const string Skip = "skip";
            public const string Sort = "sort";
            public const string Fields = "fields";
            public const string ReadQuorum = "r";
        }
    }
}
