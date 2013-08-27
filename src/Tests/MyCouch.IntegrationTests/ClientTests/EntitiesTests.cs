using System.Collections.Generic;
using System.Threading.Tasks;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class EntitiesTests : ClientTestsOf<IEntities>
    {
        protected override void OnTestInit()
        {
            SUT = Client.Entities;
        }

        [Fact]
        public void When_post_of_new_document_Using_an_entity_The_document_is_persisted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.PostAsync(artist).Result;

            response.Should().BeSuccessfulPost(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void When_put_of_new_document_Using_an_entity_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.PutAsync(artist).Result;

            response.Should().BeSuccessfulPutOfNew(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void When_put_of_existing_document_Using_an_entity_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.PostAsync(artist).Wait();

            var response = SUT.PutAsync(artist).Result;

            response.Should().BeSuccessfulPut(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void When_put_of_existing_document_Using_wrong_rev_A_conflict_is_detected()
        {
            var postResponse = SUT.PostAsync(TestData.Artists.Artist1).Result;

            postResponse.Entity.ArtistRev = "2-179d36174ee192594c63b8e8d8f09345";
            var response = SUT.PutAsync(TestData.Artists.Artist1).Result;

            response.Should().Be409Put(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_delete_of_existing_document_Using_an_entity_The_document_is_deleted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.PostAsync(artist).Wait();

            var response = SUT.DeleteAsync(artist).Result;

            response.Should().BeSuccessfulDelete(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void Flow_tests()
        {
            var artists = TestData.Artists.CreateArtists(2);
            var artist1 = artists[0];
            var artist2 = artists[1];

            var post1 = SUT.PostAsync(artist1);
            var post2 = SUT.PostAsync(artist2);

            post1.Result.Should().BeSuccessfulPost(artist1.ArtistId, e => e.ArtistId, e => e.ArtistRev);
            post2.Result.Should().BeSuccessfulPost(artist2.ArtistId, e => e.ArtistId, e => e.ArtistRev);

            var get1 = SUT.GetAsync<Artist>(post1.Result.Id);
            var get2 = SUT.GetAsync<Artist>(post2.Result.Id);

            get1.Result.Should().BeSuccessfulGet(post1.Result.Id);
            get2.Result.Should().BeSuccessfulGet(post2.Result.Id);

            get1.Result.Entity.Albums = new List<Album>(get1.Result.Entity.Albums) { new Album { Name = "Test" } }.ToArray();
            get2.Result.Entity.Albums = new List<Album>(get2.Result.Entity.Albums) { new Album { Name = "Test" } }.ToArray();

            var put1 = SUT.PutAsync(get1.Result.Entity);
            var put2 = SUT.PutAsync(get2.Result.Entity);

            put1.Result.Should().BeSuccessfulPut(get1.Result.Id, i => i.ArtistId, i => i.ArtistRev);
            put2.Result.Should().BeSuccessfulPut(get2.Result.Id, i => i.ArtistId, i => i.ArtistRev);

            SUT.DeleteAsync(put1.Result.Entity).Result.Should().BeSuccessfulDelete(put1.Result.Id, e => e.ArtistId, e => e.ArtistRev);
            SUT.DeleteAsync(put2.Result.Entity).Result.Should().BeSuccessfulDelete(put2.Result.Id, e => e.ArtistId, e => e.ArtistRev);
        }
    }
}