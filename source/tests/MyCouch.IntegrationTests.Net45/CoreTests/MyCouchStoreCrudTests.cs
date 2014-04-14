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
    }
}