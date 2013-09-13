using FluentAssertions;
using MyCouch.Commands;
using MyCouch.Testing;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class AttachmentsTests : ClientTestsOf<IAttachments>
    {
        protected override void OnTestInit()
        {
            SUT = Client.Attachments;
        }

        [Fact]
        public void When_PUT_of_a_new_attachment_and_new_document_The_response_is_ok()
        {
            var putCmd = new PutAttachmentCommand(
                ClientTestData.Artists.Artist1Id,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentAndDocResponse = SUT.PutAsync(putCmd).Result;

            putAttachmentAndDocResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_PUT_of_a_new_attachment_The_response_is_ok()
        {
            var postDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putCmd = new PutAttachmentCommand(
                postDocResponse.Id,
                postDocResponse.Rev, 
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.ContentDecoded.AsBytes());

            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            putAttachmentResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_DELETE_of_an_existing_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            var deleteCmd = new DeleteAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                ClientTestData.Attachments.One.Name);
            var deleteResponse = SUT.DeleteAsync(deleteCmd).Result;

            deleteResponse.Should().BeSuccessfulDelete(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_Using_id_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id,
                ClientTestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getCmd).Result;

            getAttachmentResponse.Should().BeSuccessfulGet(ClientTestData.Artists.Artist1Id, ClientTestData.Attachments.One.Name);
            getAttachmentResponse.Content.AsBase64EncodedString().Should().Be(ClientTestData.Attachments.One.ContentEncoded);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_Using_id_and_rev_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                ClientTestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getCmd).Result;

            getAttachmentResponse.Should().BeSuccessfulGet(ClientTestData.Artists.Artist1Id, ClientTestData.Attachments.One.Name);
            getAttachmentResponse.Content.AsBase64EncodedString().Should().Be(ClientTestData.Attachments.One.ContentEncoded);
        }

        [Fact]
        public void Flow_tests()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putCmd = new PutAttachmentCommand(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.ContentDecoded.AsBytes());
            var putAttachmentResponse = SUT.PutAsync(putCmd).Result;
            putAttachmentResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);

            var getCmd = new GetAttachmentCommand(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                ClientTestData.Attachments.One.Name);
            var getResponse = SUT.GetAsync(getCmd).Result;
            getResponse.Should().BeSuccessfulGet(ClientTestData.Artists.Artist1Id, ClientTestData.Attachments.One.Name);

            var deleteCmd = new DeleteAttachmentCommand(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                ClientTestData.Attachments.One.Name);
            var deleteResponse = SUT.DeleteAsync(deleteCmd).Result;
            deleteResponse.Should().BeSuccessfulDelete(ClientTestData.Artists.Artist1Id);
        }
    }
}