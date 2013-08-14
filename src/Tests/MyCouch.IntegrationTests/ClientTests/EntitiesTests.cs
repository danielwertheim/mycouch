using System.Collections.Generic;
using MyCouch.Testing;
using MyCouch.Testing.Model;
#if !WinRT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
using MyCouch.Extensions;

namespace MyCouch.IntegrationTests.ClientTests
{
    [TestClass]
    public class EntitiesTests : IntegrationTestsOf<IEntities>
    {
        public EntitiesTests()
        {
            OnTestInitialize = () => SUT = Client.Entities;
            OnTestFinalize = () => IntegrationTestsRuntime.ClearAllDocuments();
        }

        [TestMethod]
        public void When_post_of_new_document_Using_an_entity_The_document_is_persisted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.Post(artist);

            response.Should().BeSuccessfulPost(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [TestMethod]
        public void When_put_of_new_document_Using_an_entity_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.Put(artist);

            response.Should().BeSuccessfulPutOfNew(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [TestMethod]
        public void When_put_of_existing_document_Using_an_entity_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.Post(artist);

            var response = SUT.Put(artist);

            response.Should().BeSuccessfulPut(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [TestMethod]
        public void When_put_of_existing_document_Using_wrong_rev_A_conflict_is_detected()
        {
            var postResponse = SUT.Post(TestData.Artists.Artist1);

            postResponse.Entity.ArtistRev = "2-179d36174ee192594c63b8e8d8f09345";
            var response = SUT.Put(TestData.Artists.Artist1);

            response.Should().Be409Put(TestData.Artists.Artist1Id);
        }

        [TestMethod]
        public void When_delete_of_existing_document_Using_an_entity_The_document_is_deleted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.Post(artist);

            var response = SUT.Delete(artist);

            response.Should().BeSuccessfulDelete(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [TestMethod]
        public void Flow_tests()
        {
            var artists = TestData.Artists.CreateArtists(2);
            var artist1 = artists[0];
            var artist2 = artists[1];

            var post1 = SUT.PostAsync(artist1);
            var post2 = SUT.Post(artist2);

            post1.Result.Should().BeSuccessfulPost(artist1.ArtistId, e => e.ArtistId, e => e.ArtistRev);
            post2.Should().BeSuccessfulPost(artist2.ArtistId, e => e.ArtistId, e => e.ArtistRev);

            var get1 = SUT.GetAsync<Artist>(post1.Result.Id);
            var get2 = SUT.Get<Artist>(post2.Id);

            get1.Result.Should().BeSuccessfulGet(post1.Result.Id);
            get2.Should().BeSuccessfulGet(post2.Id);

            get1.Result.Entity.Albums = new List<Album>(get1.Result.Entity.Albums) { new Album { Name = "Test" } }.ToArray();
            get2.Entity.Albums = new List<Album>(get2.Entity.Albums) { new Album { Name = "Test" } }.ToArray();

            var put1 = SUT.PutAsync(get1.Result.Entity);
            var put2 = SUT.Put(get2.Entity);

            var delete1 = SUT.DeleteAsync(put1.Result.Entity);
            var delete2 = SUT.Delete(put2.Entity);

            delete1.Result.Should().BeSuccessfulDelete(put1.Result.Id, e => e.ArtistId, e => e.ArtistRev);
            delete2.Should().BeSuccessfulDelete(put2.Id, e => e.ArtistId, e => e.ArtistRev);
        }
    }
}