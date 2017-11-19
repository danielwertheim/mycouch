//using System.Threading.Tasks;
//using EnsureThat;
//using MyCouch.Extensions;
//using MyCouch.HttpRequestFactories;
//using MyCouch.Requests;
//using MyCouch.Responses;
//using MyCouch.Responses.Factories;
//using MyCouch.Serialization;

//namespace MyCouch.Contexts
//{
//    public class Searches : ApiContextBase<IDbConnection>, ISearches
//    {
//        protected SearchIndexHttpRequestFactory SearchIndexHttpRequestFactory { get; set; }
//        protected SearchIndexResponseFactory SearchIndexResponseFactory { get; set; }

//        public Searches(IDbConnection connection, ISerializer documentSerializer, ISerializer serializer)
//            : base(connection)
//        {
//            Ensure.That(documentSerializer, "documentSerializer").IsNotNull();
//            Ensure.Any.IsNotNull(serializer, nameof(serializer));

//            SearchIndexHttpRequestFactory = new SearchIndexHttpRequestFactory(serializer);
//            SearchIndexResponseFactory = new SearchIndexResponseFactory(documentSerializer);
//        }

//        public virtual async Task<SearchIndexResponse> SearchAsync(SearchIndexRequest request)
//        {
//            Ensure.Any.IsNotNull(request, nameof(request));

//            var httpRequest = SearchIndexHttpRequestFactory.Create(request);

//            using (var res = await SendAsync(httpRequest).ForAwait())
//            {
//                return await SearchIndexResponseFactory.CreateAsync(res).ForAwait();
//            }
//        }

//        public virtual async Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest request)
//        {
//            Ensure.Any.IsNotNull(request, nameof(request));

//            var httpRequest = SearchIndexHttpRequestFactory.Create(request);

//            using (var res = await SendAsync(httpRequest).ForAwait())
//            {
//                return await SearchIndexResponseFactory.CreateAsync<TIncludedDoc>(res).ForAwait();
//            }
//        }
//    }
//}