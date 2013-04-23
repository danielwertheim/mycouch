using MyCouch.Testing;
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
            var artist = TestDataFactory.CreateArtist();
            var initialId = artist.ArtistId;

            var response = SUT.Post(artist);

            response.Should().BeSuccessfulPost(initialId, e => e.ArtistId, e => e.ArtistRev);
        }

        [Test]
        public void When_post_of_new_document_Using_json_The_document_is_persisted()
        {
            var artist = TestDataFactory.CreateArtist();
            var json = Client.Serializer.SerializeEntity(artist);

            var response = SUT.Post(json);

            response.Should().BeSuccessfulPost(artist.ArtistId);
        }
    }
}