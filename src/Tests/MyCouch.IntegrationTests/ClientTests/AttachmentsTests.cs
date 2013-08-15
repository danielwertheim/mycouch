using FluentAssertions;
using MyCouch.Commands;
using MyCouch.Testing;
#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
using MyCouch.Extensions;

namespace MyCouch.IntegrationTests.ClientTests
{
    [TestClass]
    public class AttachmentsTests : IntegrationTestsOf<IAttachments>
    {
        public AttachmentsTests()
        {
            OnTestInitialize = () => SUT = Client.Attachments;
            OnTestFinalize = () => IntegrationTestsRuntime.ClearAllDocuments();
        }

        [TestMethod]
        public void When_PUT_of_a_new_attachment_and_new_document_The_response_is_ok()
        {
            var putCmd = new PutAttachmentCommand(
                TestData.Artists.Artist1Id,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentAndDocResponse = SUT.Put(putCmd);

            putAttachmentAndDocResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);
        }

        [TestMethod]
        public void When_PUT_of_a_new_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);
            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id, 
                putDocResponse.Rev, 
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentResponse = SUT.Put(putCmd);

            putAttachmentResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);
        }

        [TestMethod]
        public void When_DELETE_of_an_existing_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.Put(putCmd);

            var deleteCmd = new DeleteAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                TestData.Attachments.One.Name);
            var deleteResponse = SUT.Delete(deleteCmd);

            deleteResponse.Should().BeSuccessfulDelete(TestData.Artists.Artist1Id);
        }

        [TestMethod]
        public void When_GET_of_an_existing_attachment_Using_id_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.Put(putCmd);

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id,
                TestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.Get(getCmd);

            getAttachmentResponse.Should().BeSuccessfulGet(TestData.Artists.Artist1Id, TestData.Attachments.One.Name);
            getAttachmentResponse.Content.AsBase64EncodedString().Should().Be(TestData.Attachments.One.ContentEncoded);
        }

        [TestMethod]
        public void When_GET_of_an_existing_attachment_Using_id_and_rev_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.Put(putCmd);

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                TestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.Get(getCmd);

            getAttachmentResponse.Should().BeSuccessfulGet(TestData.Artists.Artist1Id, TestData.Attachments.One.Name);
            getAttachmentResponse.Content.AsBase64EncodedString().Should().Be(TestData.Attachments.One.ContentEncoded);
        }

        [TestMethod]
        public void Flow_tests()
        {
            var putDocResponse = Client.Documents.Post(TestData.Artists.Artist1Json);

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.Put(putCmd);
            putAttachmentResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                TestData.Attachments.One.Name);
            var getResponse = SUT.Get(getCmd);
            getResponse.Should().BeSuccessfulGet(TestData.Artists.Artist1Id, TestData.Attachments.One.Name);

            var deleteCmd = new DeleteAttachmentCommand(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                TestData.Attachments.One.Name);
            var deleteResponse = SUT.Delete(deleteCmd);
            deleteResponse.Should().BeSuccessfulDelete(TestData.Artists.Artist1Id);
        }
    }
}