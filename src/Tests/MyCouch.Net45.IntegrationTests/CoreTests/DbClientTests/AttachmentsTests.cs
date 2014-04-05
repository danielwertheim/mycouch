using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests.DbClientTests
{
    public class AttachmentsTests : IntegrationTestsOf<IAttachments>
    {
        public AttachmentsTests()
        {
            SUT = DbClient.Attachments;
        }

        [MyFact(TestScenarios.AttachmentsContext)]
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

        [MyFact(TestScenarios.AttachmentsContext)]
        public void When_PUT_of_a_new_attachment_The_response_is_ok()
        {
            var postDocResponse = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putRequest = new PutAttachmentRequest(
                postDocResponse.Id,
                postDocResponse.Rev, 
                ClientTestData.Attachments.One.Name,
                ClientTestData.Attachments.One.ContentType,
                ClientTestData.Attachments.One.Bytes);

            var putAttachmentResponse = SUT.PutAsync(putRequest).Result;

            putAttachmentResponse.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.AttachmentsContext)]
        public void When_DELETE_of_an_existing_attachment_The_response_is_ok()
        {
            var putDocResponse = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

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

        [MyFact(TestScenarios.AttachmentsContext)]
        public void When_GET_of_an_existing_attachment_Using_id_The_attachment_is_returned_correctly()
        {
            var putDocResponse = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

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

        [MyFact(TestScenarios.AttachmentsContext)]
        public void When_GET_of_an_existing_attachment_Using_id_and_rev_The_attachment_is_returned_correctly()
        {
            var putDocResponse = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

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

        [MyFact(TestScenarios.AttachmentsContext)]
        public void When_GET_of_an_existing_attachment_the_content_type_is_returned_correctly()
        {
            var putDocResponse = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

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

        [MyFact(TestScenarios.AttachmentsContext)]
        public void Flow_tests()
        {
            var putDocResponse = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

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