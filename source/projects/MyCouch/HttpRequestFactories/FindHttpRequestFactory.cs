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
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual HttpRequest Create(FindRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

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
            Ensure.String.IsNotNullOrWhiteSpace(request.SelectorExpression, nameof(request.SelectorExpression));

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
            if (request.Conflicts.HasValue)
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Conflicts, Serializer.ToJson(request.Conflicts.Value));
            if (request.HasBookmark())
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Bookmark, Serializer.ToJson(request.Bookmark));
            if (request.Stable.HasValue)
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Stable, Serializer.ToJson(request.Stable.Value));
            if (request.Update.HasValue)
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.Update, Serializer.ToJson(request.Update.Value));
            if (request.HasUseIndex())
                sb.AppendFormat(FormatStrings.JsonPropertyAppendFormat, KeyNames.UseIndex, Serializer.ToJson(request.UseIndex));

            sb.Append("}");

            return sb.ToString();
        }

        protected static class KeyNames
        {
            public const string Selector = "selector";
            public const string Limit = "limit";
            public const string Skip = "skip";
            public const string Sort = "sort";
            public const string Fields = "fields";
            public const string ReadQuorum = "r";
            public const string Conflicts = "conflicts";
            public const string Bookmark = "bookmark";
            public const string Stable = "stable";
            public const string Update = "update";
            public const string UseIndex = "use_index";
        }
    }
}
