using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
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
        public void When_post_of_a_new_entity_Then_the_document_is_created()
        {
            var artist = ClientTestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.PostAsync(artist).Result;

            response.Should().BeSuccessfulPost(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void When_put_of_a_new_entity_Then_the_document_is_created()
        {
            var artist = ClientTestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.PutAsync(artist).Result;

            response.Should().BeSuccessfulPutOfNew(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void When_put_of_an_existing_entity_Then_the_document_is_replaced()
        {
            var artist = ClientTestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.PostAsync(artist).Wait();

            var response = SUT.PutAsync(artist).Result;

            response.Should().BeSuccessfulPut(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void When_put_of_an_existing_entity_Using_wrong_rev_Then_a_conflict_is_detected()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1).Result;

            postResponse.Entity.ArtistRev = "2-179d36174ee192594c63b8e8d8f09345";
            var response = SUT.PutAsync(ClientTestData.Artists.Artist1).Result;

            response.Should().Be409Put(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_delete_of_an_existing_entity_Then_the_document_is_deleted()
        {
            var artist = ClientTestData.Artists.CreateArtist();
            var initialId = artist.ArtistId;
            SUT.PostAsync(artist).Wait();

            var response = SUT.DeleteAsync(artist).Result;

            response.Should().BeSuccessfulDelete(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Fact]
        public void Flow_tests()
        {
            var artists = ClientTestData.Artists.CreateArtists(2);
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

        [Fact]
        public void When_put_of_a_new_entity_extending_a_document_Then_the_entity_will_be_created()
        {
            var id = "956cba86-a20c-4a85-a8b4-a7039ba771c8";
            var entity = new Inherited { _id = id, Value = "Test" };

            var response = SUT.PutAsync(entity).Result;

            response.Should().BeSuccessfulPutOfNew(id, i => i._id, i => i._rev);
        }

        [Fact]
        public void When_put_of_an_existing_entity_extending_a_document_Then_the_entity_will_be_updated()
        {
            var id = "b638d1c5-772a-48f4-b6ee-f2c1f7d5e410";
            var entity = new Inherited { _id = id, Value = "Test" };
            SUT.PostAsync(entity).Wait();

            var response = SUT.PutAsync(entity).Result;

            response.Should().BeSuccessfulPut(id, i => i._id, i => i._rev);
        }

        [Fact]
        public void When_put_of_a_new_and_then_a_put_of_an_existing_entity_extending_a_document_Then_the_entity_will_first_be_created_and_then_updated()
        {
            var id = "b638d1c5-772a-48f4-b6ee-f2c1f7d5e410";
            var entity = new Inherited { _id = id, Value = "Test" };

            var putResponse1 = SUT.PutAsync(entity).Result;
            putResponse1.Should().BeSuccessfulPutOfNew(id, i => i._id, i => i._rev);

            var putResponse2 = SUT.PutAsync(entity).Result;
            putResponse2.Should().BeSuccessfulPut(id, i => i._id, i => i._rev);
        }

        private abstract class Document
        {
            public string _id { get; set; }
            public string _rev { get; set; }
        }

        private class Inherited : Document
        {
            public string Value { get; set; }
        }
    }
}