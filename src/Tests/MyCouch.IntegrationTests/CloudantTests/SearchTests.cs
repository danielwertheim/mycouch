using MyCouch.Cloudant;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.IntegrationTests.CloudantTests
{
    public class SearchTests : CloudantTestsOf<ISearch>, IPreserveStatePerFixture, IUseFixture<SearchFixture>
    {
        protected Animal[] Animals { get; set; }

        protected override void OnTestInit()
        {
            SUT = Client.Search;
        }

        public void SetFixture(SearchFixture data)
        {
            Animals = data.Animals;
        }
    }
}
