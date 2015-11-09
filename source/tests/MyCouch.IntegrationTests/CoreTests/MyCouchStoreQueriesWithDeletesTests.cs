using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class MyCouchStoreQueriesWithDeletesTests : IntegrationTestsOf<MyCouchStore>,
        IPreserveStatePerFixture,
        IClassFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }
        protected Artist[] ArtistsNotDeleted { get; set; }
        protected Artist ArtistBeingDeleted { get; set; }

        public MyCouchStoreQueriesWithDeletesTests(ViewsFixture data)
        {
            ArtistsById = data.Init(Environment);
            SUT = new MyCouchStore(DbClient);
            ArtistBeingDeleted = ArtistsById.First();
            ArtistsNotDeleted = ArtistsById.Where(a => a != ArtistBeingDeleted).ToArray();
            SUT.DeleteAsync(ArtistBeingDeleted).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetHeadersAync_via_callback_When_getting_one_existing_and_one_deleted_header_It_returns_only_the_existing_one()
        {
            var artists = new[] { ArtistsNotDeleted[0], ArtistBeingDeleted };
            var ids = artists.Select(a => a.ArtistId).ToArray();
            var headers = new List<DocumentHeader>();

            SUT.GetHeadersAsync(ids, headers.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var expectedArtistHeaders = artists
                    .Where(a => a != ArtistBeingDeleted)
                    .Select(a => new DocumentHeader(a.ArtistId, a.ArtistRev))
                    .ToList();

                headers.ShouldBeEquivalentTo(expectedArtistHeaders);
            }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetHeadersAync_via_result_When_getting_one_existing_and_one_deleted_header_It_returns_only_the_existing_one()
        {
            var artists = new[] { ArtistsNotDeleted[0], ArtistBeingDeleted };
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var headers = SUT.GetHeadersAsync(ids).Result;

            var expectedArtistHeaders = artists
                .Where(a => a != ArtistBeingDeleted)
                .Select(a => new DocumentHeader(a.ArtistId, a.ArtistRev))
                .ToList();
            headers.ShouldBeEquivalentTo(expectedArtistHeaders);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetByIdsAync_via_callback_When_getting_one_existing_and_one_deleted_header_It_returns_only_the_existing_one()
        {
            var artists = new[] { ArtistsNotDeleted[0], ArtistBeingDeleted };
            var ids = artists.Select(a => a.ArtistId).ToArray();
            var matches = new List<Artist>();

            SUT.GetByIdsAsync<Artist>(ids, matches.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var expectedArtists = artists
                    .Where(a => a != ArtistBeingDeleted)
                    .ToList();

                matches.ShouldBeEquivalentTo(expectedArtists);
            }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetByIdsAync_via_result_When_getting_one_existing_and_one_deleted_header_It_returns_only_the_existing_one()
        {
            var artists = new[] { ArtistsNotDeleted[0], ArtistBeingDeleted };
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var matches = SUT.GetByIdsAsync<Artist>(ids).Result;

            var expectedArtists = artists
                .Where(a => a != ArtistBeingDeleted)
                .ToList();
            matches.ShouldBeEquivalentTo(expectedArtists);
        }
    }
}
