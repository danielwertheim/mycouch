using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyCouch.Querying;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using NUnit.Framework;

namespace MyCouch.IntegrationTests.Documents
{
    [TestFixture]
    public class ViewsTests : IntegrationTestsOf<IViews>
    {
        protected override void OnFixtureInitialize()
        {
            base.OnFixtureInitialize();

            Client.Documents.Post(TestData.Views.ArtistsAlbums);
        }

        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = Client.Views;
        }

        protected override void OnTestFinalize()
        {
            base.OnTestFinalize();

            IntegrationTestsRuntime.ClearAllDocuments();
        }

        [Test]
        public void When_Skip_and_Limit_to_get_range_Then_range_is_returned()
        {
            var items = TestData.CreateArtists(10);
            var t = items.Select(item => Client.Documents.PostAsync(item)).ToArray();
            Task.WaitAll(t);

            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Skip(2).Limit(2));
            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(2);
        }
    }
}