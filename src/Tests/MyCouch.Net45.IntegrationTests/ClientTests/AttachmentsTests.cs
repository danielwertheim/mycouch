using FluentAssertions;
using MyCouch.Requests;
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
            var putRequest = new PutAttachmentRequest(
                ClientTestData.Artists.Artist1Id,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);

            var putAttachmentAndDocResponse = SUT.PutAsync(putRequest).Result;

            putAttachmentAndDocResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_PUT_of_a_new_attachment_The_response_is_ok()
        {
            var postDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putRequest = new PutAttachmentRequest(
                postDocResponse.Id,
                postDocResponse.Rev, 
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);

            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;

            putAttachmentResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_DELETE_of_an_existing_attachment_The_response_is_ok()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putRequest = new PutAttachmentRequest(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);
            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;

            var deleteRequest = new DeleteAttachmentRequest(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                ClientTestData.Attachments.One.Name);
            var deleteResponse = SUT.DeleteAsync(deleteRequest).Result;

            deleteResponse.Should().BeSuccessfulDelete(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_Using_id_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putRequest = new PutAttachmentRequest(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);
            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;

            var getRequest = new GetAttachmentRequest(
                putAttachmentResponse.Id,
                ClientTestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getRequest).Result;

            getAttachmentResponse.Should().BeSuccessfulGet(ClientTestData.Artists.Artist1Id, ClientTestData.Attachments.One.Name);
            getAttachmentResponse.Content.Should().Equal(ClientTestData.Attachments.One.Bytes);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_Using_id_and_rev_The_attachment_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putRequest = new PutAttachmentRequest(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);
            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;

            var getRequest = new GetAttachmentRequest(
                putAttachmentResponse.Id, 
                putAttachmentResponse.Rev, 
                ClientTestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getRequest).Result;

            getAttachmentResponse.Should().BeSuccessfulGet(ClientTestData.Artists.Artist1Id, ClientTestData.Attachments.One.Name);
            getAttachmentResponse.Content.Should().Equal(ClientTestData.Attachments.One.Bytes);
        }

        [Fact]
        public void When_GET_of_an_existing_attachment_the_content_type_is_returned_correctly()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putRequest = new PutAttachmentRequest(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);
            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;

            var getRequest = new GetAttachmentRequest(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                ClientTestData.Attachments.One.Name);
            var getAttachmentResponse = SUT.GetAsync(getRequest).Result;

            getAttachmentResponse.ContentType.Should().Be(ClientTestData.Attachments.One.ContentType);
        }

        [Fact]
        public void Flow_tests()
        {
            var putDocResponse = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var putRequest = new PutAttachmentRequest(
                putDocResponse.Id,
                putDocResponse.Rev,
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);
            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;
            putAttachmentResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);

            var getRequest = new GetAttachmentRequest(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                ClientTestData.Attachments.One.Name);
            var getResponse = SUT.GetAsync(getRequest).Result;
            getResponse.Should().BeSuccessfulGet(ClientTestData.Artists.Artist1Id, ClientTestData.Attachments.One.Name);

            var deleteRequest = new DeleteAttachmentRequest(
                putAttachmentResponse.Id,
                putAttachmentResponse.Rev,
                ClientTestData.Attachments.One.Name);
            var deleteResponse = SUT.DeleteAsync(deleteRequest).Result;
            deleteResponse.Should().BeSuccessfulDelete(ClientTestData.Artists.Artist1Id);
        }
    }
}