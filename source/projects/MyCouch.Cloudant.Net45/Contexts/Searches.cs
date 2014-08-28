using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
	public class Searches : ApiContextBase<IDbClientConnection>, ISearches
	{
		protected SearchIndexHttpRequestFactory SearchIndexHttpRequestFactory { get; set; }
		protected SearchIndexResponseFactory SearchIndexResponseFactory { get; set; }

		public Searches(IDbClientConnection connection, ISerializer serializer)
			: base(connection)
		{
			Ensure.That(serializer, "serializer").IsNotNull();

			SearchIndexHttpRequestFactory = new SearchIndexHttpRequestFactory(serializer);
			SearchIndexResponseFactory = new SearchIndexResponseFactory(serializer);
		}

		public virtual async Task<SearchIndexResponse> SearchAsync(SearchIndexRequest request)
		{
			Ensure.That(request, "request").IsNotNull();

			var httpRequest = CreateHttpRequest(request);

			using (var res = await SendAsync(httpRequest).ForAwait())
			{
				return ProcessHttpResponse(res);
			}
		}

		public virtual async Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest request)
		{
			Ensure.That(request, "request").IsNotNull();

			var httpRequest = CreateHttpRequest(request);

			using (var res = await SendAsync(httpRequest).ForAwait())
			{
				return ProcessHttpResponse<TIncludedDoc>(res);
			}
		}

		protected virtual SearchIndexRequest CreateSearchIndexRequest(string designDocument, string searchIndexName)
		{
			return new SearchIndexRequest(designDocument, searchIndexName);
		}

		protected virtual HttpRequest CreateHttpRequest(SearchIndexRequest request)
		{
			return SearchIndexHttpRequestFactory.Create(request);
		}

		protected virtual SearchIndexResponse ProcessHttpResponse(HttpResponseMessage response)
		{
			return SearchIndexResponseFactory.Create(response);
		}

		protected virtual SearchIndexResponse<TIncludedDoc> ProcessHttpResponse<TIncludedDoc>(HttpResponseMessage response)
		{
			return SearchIndexResponseFactory.Create<TIncludedDoc>(response);
		}
	}
}