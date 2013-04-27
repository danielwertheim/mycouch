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
        protected Artist[] Artists;

        protected override void OnFixtureInitialize()
        {
            base.OnFixtureInitialize();

            Artists = TestData.Artists.CreateArtists(10);
            
            var tasks = new List<Task>();
            tasks.AddRange(Artists.Select(item => Client.Documents.PostAsync(item)));
            tasks.Add(Client.Documents.PostAsync(TestData.Views.ArtistsAlbums));
            
            Task.WaitAll(tasks.ToArray());
        }

        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = Client.Views;
        }

        protected override void OnFixtureFinalize()
        {
            base.OnFixtureFinalize();

            IntegrationTestsRuntime.ClearAllDocuments();
        }

        [Test]
        public void When_Skipping_2_of_10_Then_8_rows_are_returned()
        {
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Skip(2));
            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(8);
        }

        [Test]
        public void When_Limit_to_2_Then_2_rows_are_returned()
        {
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Limit(2));
            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(2);
        }

        [Test]
        public void When_Key_is_specified_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Key(artist.Name));
            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(1);
            response.Rows[0].Id.Should().Be(artist.ArtistId);
            response.Rows[0].Key.Should().Be(artist.Name);
        }
    }
}