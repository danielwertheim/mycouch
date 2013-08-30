using MyCouch.Cloudant;

namespace MyCouch.IntegrationTests.CloudantTests
{
    public class SearchTests : CloudantTestsOf<ISearch>
    {
        protected override void OnTestInit()
        {
            SUT = Client.Search;
        }
    }
}
