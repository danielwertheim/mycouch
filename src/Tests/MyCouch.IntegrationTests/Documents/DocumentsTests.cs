using System.Collections.Generic;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using NUnit.Framework;

namespace MyCouch.IntegrationTests.Documents
{
    [TestFixture]
    public class DocumentsTests : IntegrationTestsOf<IDocuments>
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = Client.Documents;
        }

        protected override void OnTestFinalize()
        {
            base.OnTestFinalize();

            IntegrationTestsRuntime.ClearAllDocuments();
        }

        [Test]
        public void When_post_of_new_document_Using_an_entity_The_document_is_persisted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.Post(artist);

            response.Should().BeSuccessfulPost(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Test]
        public void When_post_of_new_document_Using_json_The_document_is_persisted()
        {
            var artist = TestData.Artists.CreateArtist();
            var json = Client.Serializer.SerializeEntity(artist);

            var response = SUT.Post(json);

            response.Should().BeSuccessfulPost(artist.ArtistId);
        }

        [Test]
        public void When_put_of_new_document_Using_an_entity_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.Put(artist);

            response.Should().BeSuccessfulPutOfNew(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Test]
        public void When_put_of_new_document_Using_json_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            var json = Client.Serializer.SerializeEntity(artist);

            var response = SUT.Put(initialId, json);

            response.Should().BeSuccessfulPutOfNew(initialId);
        }

        [Test]
        public void When_put_of_existing_document_Using_an_entity_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.Post(artist);

            var response = SUT.Put(artist);

            response.Should().BeSuccessfulPut(initialId, e => e.ArtistId, e => e.ArtistRev);
        }
        
        [Test]
        public void When_put_of_existing_document_Using_json_The_document_is_replaced()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.Post(artist);
            var json = Client.Serializer.SerializeEntity(artist);

            var response = SUT.Put(initialId, json);

            response.Should().BeSuccessfulPut(initialId);
        }

        [Test]
        public void When_delete_of_existing_document_Using_an_entity_The_document_is_deleted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.Post(artist);

            var response = SUT.Delete(artist);

            response.Should().BeSuccessfulDelete(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Test]
        public void When_delete_of_existing_document_Using_id_and_rev_The_document_is_deleted()
        {
            var artist = TestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.Post(artist);

            var response = SUT.Delete(artist.ArtistId, artist.ArtistRev);

            response.Should().BeSuccessfulDelete(initialId);
        }

        [Test]
        public void CRUD_using_non_typed_API()
        {
            var post1 = SUT.PostAsync(TestData.Artists.Artist1Json).Result;
            post1.Should().BeSuccessfulPost(TestData.Artists.Artist1Id);

            var post2 = SUT.Post(TestData.Artists.Artist2Json);
            post2.Should().BeSuccessfulPost(TestData.Artists.Artist2Id);

            var get1 = SUT.GetAsync(post1.Id).Result;
            get1.Should().BeSuccessfulGet(post1.Id);

            var get2 = SUT.Get(post2.Id);
            get2.Should().BeSuccessfulGet(post2.Id);

            var kv1 = Client.Serializer.Deserialize<IDictionary<string, dynamic>>(get1.Content);
            kv1["year"] = 2000;
            var docUpd1 = Client.Serializer.Serialize(kv1);
            var put1 = SUT.PutAsync(get1.Id, docUpd1).Result;
            put1.Should().BeSuccessfulPut(get1.Id);

            var kv2 = Client.Serializer.Deserialize<IDictionary<string, dynamic>>(get2.Content);
            kv2["year"] = 2001;
            var docUpd2 = Client.Serializer.Serialize(kv2);
            var put2 = SUT.Put(get2.Id, docUpd2);
            put2.Should().BeSuccessfulPut(get2.Id);

            var delete1 = SUT.DeleteAsync(put1.Id, put1.Rev).Result;
            delete1.Should().BeSuccessfulDelete(put1.Id);

            var delete2 = SUT.Delete(put2.Id, put2.Rev);
            delete2.Should().BeSuccessfulDelete(put2.Id);
        }

        [Test]
        public void CRUD_using_typed_API()
        {
            var artists = TestData.Artists.CreateArtists(2);
            var artist1 = artists[0];
            var artist2 = artists[1];

            var post1 = SUT.PostAsync(artist1).Result;
            post1.Should().BeSuccessfulPost(artist1.ArtistId, e => e.ArtistId, e => e.ArtistRev);

            var post2 = SUT.Post(artist2);
            post2.Should().BeSuccessfulPost(artist2.ArtistId, e => e.ArtistId, e => e.ArtistRev);

            var get1 = SUT.GetAsync<Artist>(post1.Id).Result;
            get1.Should().BeSuccessfulGet(post1.Id);

            var get2 = SUT.Get<Artist>(post2.Id);
            get2.Should().BeSuccessfulGet(post2.Id);

            get1.Entity.Albums = new List<Album>(get1.Entity.Albums) { new Album { Name = "Test" } }.ToArray();
            var put1 = SUT.PutAsync(get1.Entity).Result;

            get2.Entity.Albums = new List<Album>(get2.Entity.Albums) { new Album { Name = "Test" } }.ToArray();
            var put2 = SUT.Put(get2.Entity);

            var delete1 = SUT.DeleteAsync(put1.Entity).Result;
            delete1.Should().BeSuccessfulDelete(put1.Id, e => e.ArtistId, e => e.ArtistRev);

            var delete2 = SUT.Delete(put2.Entity);
            delete2.Should().BeSuccessfulDelete(put2.Id, e => e.ArtistId, e => e.ArtistRev);
        }
    }
}