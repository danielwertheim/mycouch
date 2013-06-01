using MyCouch.Testing;
using NUnit.Framework;

namespace MyCouch.IntegrationTests.ClientTests
{
    [TestFixture]
    public class AttachmentsTests : IntegrationTestsOf<IAttachments>
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = Client.Attachments;
        }

        protected override void OnTestFinalize()
        {
            base.OnTestFinalize();

            IntegrationTestsRuntime.ClearAllDocuments();
        }

        [Test]
        public void When_PUT_of_a_new_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);
            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id, 
                putDocResponse.Rev, 
                TestData.Attachments.One.Id,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentResponse = SUT.Put(putCmd);

            putAttachmentResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);
        }

        [Test]
        public void When_DELETE_of_an_existing_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);
            var deleteCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Id,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.Put(deleteCmd);

            var deleteResponse = SUT.Delete(new DeleteAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev,
                TestData.Attachments.One.Id));

            deleteResponse.Should().BeSuccessfulDelete(TestData.Artists.Artist1Id);
        }
    }
}