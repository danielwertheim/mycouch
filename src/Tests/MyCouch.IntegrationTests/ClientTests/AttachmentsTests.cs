using FluentAssertions;
using MyCouch.Commands;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class AttachmentsTests : IntegrationTestsOf<IAttachments>
    {
        public AttachmentsTests()
        {
            SUT = Client.Attachments;
        }

        [Fact]
        public void When_PUT_of_a_new_attachment_and_new_document_The_response_is_ok()
        {
            var putCmd = new PutAttachmentCommand(
                TestData.Artists.Artist1Id,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentAndDocResponse = SUT.PutAsync(putCmd).Result;

            putAttachmentAndDocResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_PUT_of_a_new_attachment_The_response_is_ok()
        {
            var postDocResponse = Client.Documents.PostAsync(TestData.Artists.Artist1Json).Result;
            var putCmd = new PutAttachmentCommand(
                postDocResponse.Id,
                postDocResponse.Rev, 
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            putAttachmentResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_DELETE_of_an_existing_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.PostAsync(TestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            var deleteCmd = new DeleteAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                TestData.Attachments.One.Name);
            var deleteResponse = SUT.DeleteAsync(deleteCmd).Result;

            deleteResponse.Should().BeSuccessfulDelete(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_Using_id_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(TestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id,
                TestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getCmd).Result;

            getAttachmentResponse.Should().BeSuccessfulGet(TestData.Artists.Artist1Id, TestData.Attachments.One.Name);
            getAttachmentResponse.Content.AsBase64EncodedString().Should().Be(TestData.Attachments.One.ContentEncoded);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_Using_id_and_rev_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(TestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                TestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getCmd).Result;

            getAttachmentResponse.Should().BeSuccessfulGet(TestData.Artists.Artist1Id, TestData.Attachments.One.Name);
            getAttachmentResponse.Content.AsBase64EncodedString().Should().Be(TestData.Attachments.One.ContentEncoded);
        }

        [Fact]
        public void Flow_tests()
        {
            var putDocResponse = Client.Documents.PostAsync(TestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                TestData.Attachments.One.Name,
                TestData.Attachments.One.ContentType,
                TestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;
            putAttachmentResponse.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                TestData.Attachments.One.Name);
            var getResponse = SUT.GetAsync(getCmd).Result;
            getResponse.Should().BeSuccessfulGet(TestData.Artists.Artist1Id, TestData.Attachments.One.Name);

            var deleteCmd = new DeleteAttachmentCommand(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                TestData.Attachments.One.Name);
            var deleteResponse = SUT.DeleteAsync(deleteCmd).Result;
            deleteResponse.Should().BeSuccessfulDelete(TestData.Artists.Artist1Id);
        }
    }
}