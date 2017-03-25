//using System.Net.Http;
//using System.Threading.Tasks;
//using MyCouch.EnsureThat;
//using MyCouch.Extensions;
//using MyCouch.Serialization;

//namespace MyCouch.Responses.Materializers
//{
//    public class SearchIndexResponseMaterializer
//    {
//        protected readonly ISerializer Serializer;

//        public SearchIndexResponseMaterializer(ISerializer serializer)
//        {
//            Ensure.That(serializer, "serializer").IsNotNull();

//            Serializer = serializer;
//        }

//        public virtual async Task MaterializeAsync<TIncludedDoc>(SearchIndexResponse<TIncludedDoc> response, HttpResponseMessage httpResponse)
//        {
//            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
//            {
//                Serializer.Populate(response, content);
//            }
//        }
//    }
//}