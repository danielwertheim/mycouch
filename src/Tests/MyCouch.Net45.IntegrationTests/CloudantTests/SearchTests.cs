using MyCouch.Cloudant;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.IntegrationTests.CloudantTests
{
    public class SearchTests : CloudantTestsOf<ISearches>, IPreserveStatePerFixture, IUseFixture<SearchFixture>
    {
        protected Animal[] Animals { get; set; }

        protected override void OnTestInit()
        {
            SUT = Client.Searches;
        }

        public void SetFixture(SearchFixture data)
        {
            Animals = data.Animals;
        }

        [Fact]
        public void When_running_unnamed_query_It_will_consume_the_default_index()
        {
            //var iq = new SearchQuery(CloudantTestData.Views.Views101AnimalsIndexId).Configure(q => q.Expression("kookaburra"));

            //var response = SUT.QueryAsync(iq).Result;
        }
    }
}