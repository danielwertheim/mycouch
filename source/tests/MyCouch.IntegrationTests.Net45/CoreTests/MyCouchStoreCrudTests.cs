using FluentAssertions;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class MyCouchStoreCrudTests : IntegrationTestsOf<MyCouchStore>
    {
        public MyCouchStoreCrudTests()
        {
            SUT = new MyCouchStore(DbClient);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void FlowTestOfJson()
        {
            var storeJson = SUT.StoreAsync(ClientTestData.Artists.Artist1Json);
            storeJson.Result.Id.Should().Be(ClientTestData.Artists.Artist1Id);
            storeJson.Result.Rev.Should().NotBeNullOrEmpty();

            var getJsonById = SUT.GetByIdAsync(storeJson.Result.Id);
            getJsonById.Result.Should().NotBeNullOrEmpty();

            var getJsonByIdAndRev = SUT.GetByIdAsync(storeJson.Result.Id, storeJson.Result.Rev);
            getJsonByIdAndRev.Result.Should().NotBeNullOrEmpty();

            var deleteByIdAndRev = SUT.DeleteAsync(storeJson.Result.Id, storeJson.Result.Rev);
            deleteByIdAndRev.Result.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void FlowTestOfEntities()
        {
            var storeEntity = SUT.StoreAsync(ClientTestData.Artists.Artist2);

            storeEntity.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            storeEntity.Result.ArtistRev.Should().NotBeNullOrEmpty();

            var getEntityById = SUT.GetByIdAsync<Artist>(storeEntity.Result.ArtistId);
            getEntityById.Result.Should().NotBeNull();
            getEntityById.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            getEntityById.Result.ArtistRev.Should().Be(storeEntity.Result.ArtistRev);

            var getEntityByIdAndRev = SUT.GetByIdAsync<Artist>(storeEntity.Result.ArtistId, storeEntity.Result.ArtistRev);
            getEntityByIdAndRev.Result.Should().NotBeNull();
            getEntityByIdAndRev.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            getEntityByIdAndRev.Result.ArtistRev.Should().Be(storeEntity.Result.ArtistRev);

            var deleteByEntity = SUT.DeleteAsync(getEntityById.Result);
            deleteByEntity.Result.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void SetAsync_When_new_document_It_inserts_the_document()
        {
            var setJson = SUT.SetAsync(ClientTestData.Artists.Artist1Id, ClientTestData.Artists.Artist1Json).Result;

            setJson.Id.Should().Be(ClientTestData.Artists.Artist1Id);
            setJson.Rev.Should().NotBeNullOrWhiteSpace();

            var get = SUT.GetByIdAsync<Artist>(setJson.Id).Result;
            get.ArtistId.Should().Be(ClientTestData.Artists.Artist1Id);
            get.ArtistRev.Should().Be(setJson.Rev);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void SetAsync_When_existing_document_It_overwrites_the_document()
        {
            var setJson1 = SUT.SetAsync(ClientTestData.Artists.Artist1Id, ClientTestData.Artists.Artist1Json).Result;

            var setJson2 = SUT.SetAsync(setJson1.Id, "{\"message\":\"I rule\"}").Result;

            setJson2.Id.Should().Be(ClientTestData.Artists.Artist1Id);
            setJson2.Rev.Should().NotBeNullOrWhiteSpace();

            var get = SUT.GetByIdAsync<Temp>(setJson2.Id).Result;
            get.Id.Should().Be(ClientTestData.Artists.Artist1Id);
            get.Rev.Should().Be(setJson2.Rev);
            get.Message.Should().Be("I rule");
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void SetAsync_When_new_entity_It_inserts_the_entity()
        {
            var artist = SUT.SetAsync(ClientTestData.Artists.Artist1).Result;

            artist.ArtistId.Should().Be(ClientTestData.Artists.Artist1Id);
            artist.ArtistRev.Should().NotBeNullOrWhiteSpace();

            var get = SUT.GetByIdAsync<Artist>(artist.ArtistId).Result;
            get.ArtistId.Should().Be(ClientTestData.Artists.Artist1Id);
            get.ArtistRev.Should().Be(artist.ArtistRev);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void SetAsync_When_existing_entity_It_overwrites_the_entity()
        {
            var artist = SUT.SetAsync(ClientTestData.Artists.Artist1).Result;

            var artist2 = SUT.SetAsync(new Artist { ArtistId = artist.ArtistId, Name = "I will overwrite without a REV." }).Result;

            artist2.ArtistId.Should().Be(ClientTestData.Artists.Artist1Id);
            artist2.ArtistRev.Should().NotBeNullOrWhiteSpace();

            var get = SUT.GetByIdAsync<Artist>(artist2.ArtistId).Result;
            get.ArtistId.Should().Be(ClientTestData.Artists.Artist1Id);
            get.ArtistRev.Should().Be(artist2.ArtistRev);
            get.Name.Should().Be("I will overwrite without a REV.");
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void DeleteAsync_When_document_does_not_exist_It_returns_false()
        {
            var deleted = SUT.DeleteAsync("7a4b2d4d66e9484bbd50f5dfadd099f9", "foo_rev").Result;

            deleted.Should().BeFalse();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void DeleteAsync_When_entity_does_not_exist_It_returns_false()
        {
            var deleted = SUT.DeleteAsync(new Artist { ArtistId = "7a4b2d4d66e9484bbd50f5dfadd099f9", ArtistRev = "foo_rev" }).Result;

            deleted.Should().BeFalse();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void DeleteAsync_When_document_exists_It_returns_true_and_deletes_the_document()
        {
            var store = SUT.StoreAsync(ClientTestData.Artists.Artist1Json).Result;

            var deleted = SUT.DeleteAsync(store.Id, store.Rev).Result;
            var exists = SUT.ExistsAsync(store.Id).Result;

            exists.Should().BeFalse();
            deleted.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void DeleteAsync_When_entity_exists_It_returns_true_and_deletes_the_document()
        {
            var stored = SUT.StoreAsync(ClientTestData.Artists.Artist1).Result;

            var deleted = SUT.DeleteAsync(stored).Result;
            var exists = SUT.ExistsAsync(stored.ArtistId).Result;

            exists.Should().BeFalse();
            deleted.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void DeleteAsync_When_no_rev_is_passed_and_document_exists_It_returns_true_and_deletes_the_document()
        {
            var store = SUT.StoreAsync(ClientTestData.Artists.Artist1Json).Result;

            var deleted = SUT.DeleteAsync(store.Id).Result;
            var exists = SUT.ExistsAsync(store.Id).Result;

            exists.Should().BeFalse();
            deleted.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void DeleteAsync_When_no_rev_is_passed_and_entity_exists_It_returns_true_and_deletes_the_document()
        {
            var stored = SUT.StoreAsync(ClientTestData.Artists.Artist1).Result;

            var deleted = SUT.DeleteAsync(new Artist { ArtistId = stored.ArtistId }, true).Result;
            var exists = SUT.ExistsAsync(stored.ArtistId).Result;

            exists.Should().BeFalse();
            deleted.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void ExistsAsync_When_document_exists_It_returns_true()
        {
            var store = SUT.StoreAsync(ClientTestData.Artists.Artist1Json).Result;

            var exists = SUT.ExistsAsync(store.Id).Result;

            exists.Should().BeTrue();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void ExistsAsync_When_document_does_not_exist_It_returns_false()
        {
            var exists = SUT.ExistsAsync("fb27a173c44748cfb5458414ef0f82ea").Result;

            exists.Should().BeFalse();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public virtual void GetHeaderAsync_When_document_exists_It_returns_the_header()
        {
            var store = SUT.StoreAsync(ClientTestData.Artists.Artist1Json).Result;

            var header = SUT.GetHeaderAsync(store.Id).Result;

            header.Should().NotBeNull();
            header.Id.Should().Be(store.Id);
            header.Rev.Should().Be(store.Rev);
        }

        private class Temp
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Message { get; set; }
        }
    }
}