﻿using System.Collections.Generic;
using FluentAssertions;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace IntegrationTests.CoreTests
{
    public class DocumentsTests : IntegrationTestsOf<IDocuments>
    {
        public DocumentsTests()
        {
            SUT = DbClient.Documents;
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_GET_of_document_with_no_conflicts_when_including_conflicts_It_returns_the_document()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var request = new GetDocumentRequest(postResponse.Id) { Conflicts = true };

            var response = SUT.GetAsync(request).Result;

            response.Should().BeSuccessfulGet(postResponse.Id, postResponse.Rev);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_GET_of_document_when_including_revisions_It_returns_at_least_its_revision()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var postResponseRevId = postResponse.Rev.Split('-')[1];
            var request = new GetDocumentRequest(postResponse.Id) { Revisions = true };

            var response = SUT.GetAsync(request).Result;

            response.Should().BeSuccessfulGet(postResponse.Id, postResponse.Rev);
            response.Revisions.Should().NotBeNull();
            response.Revisions.Ids.Should().NotBeNullOrEmpty().And.Contain(postResponseRevId);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_HEAD_of_non_existing_document_The_response_is_empty()
        {
            var response = SUT.HeadAsync("fooId").Result;

            response.Should().BeHead404("fooId");
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_HEAD_using_not_matching_rev_The_response_is_empty()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.HeadAsync(postResponse.Id, "1-795258d03c3bdb58fffc409e153c5d45").Result;

            response.Should().BeHead404(postResponse.Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_HEAD_using_matching_id_The_response_is_ok()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.HeadAsync(postResponse.Id).Result;

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_HEAD_of_existing_design_document_id_The_response_is_ok()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Views.ArtistsViews).Result;

            var response = SUT.HeadAsync(postResponse.Id).Result;

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_HEAD_using_matching_id_and_rev_The_response_is_ok()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.HeadAsync(postResponse.Id, postResponse.Rev).Result;

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_POST_of_new_document_The_document_is_persisted()
        {
            var response = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            response.Should().BeSuccessfulPost(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_POST_of_new_document_in_batch_mode_The_document_is_persisted()
        {
            var request = new PostDocumentRequest(ClientTestData.Artists.Artist1Json) { Batch = true };
            var response = SUT.PostAsync(request).Result;

            response.Should().BeSuccessfulBatchPost(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_PUT_of_new_document_The_document_is_replaced()
        {
            var response = SUT.PutAsync(ClientTestData.Artists.Artist1Id, ClientTestData.Artists.Artist1Json).Result;

            response.Should().BeSuccessfulPutOfNew(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_PUT_of_new_document_in_batch_mode_The_document_is_replaced()
        {
            var request = PutDocumentRequest.ForCreate(ClientTestData.Artists.Artist1Id, ClientTestData.Artists.Artist1Json, r => r.Batch = true);
            var response = SUT.PutAsync(request).Result;

            response.Should().BeSuccessfulBatchPutOfNew(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_PUT_of_existing_document_The_document_is_replaced()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var getResponse = SUT.GetAsync(postResponse.Id).Result;

            var response = SUT.PutAsync(getResponse.Id, getResponse.Content).Result;

            response.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void Can_PUT_of_existing_document_in_batch_mode()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var getResponse = SUT.GetAsync(postResponse.Id).Result;

            var updateRequest = PutDocumentRequest.ForCreate(getResponse.Id, getResponse.Content, r => r.Batch = true);
            var response = SUT.PutAsync(updateRequest).Result;

            response.Should().BeSuccessfulBatchPut(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_PUT_of_existing_document_Using_wrong_rev_A_conflict_is_detected()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.PutAsync(postResponse.Id, "2-179d36174ee192594c63b8e8d8f09345", ClientTestData.Artists.Artist1Json).Result;

            response.Should().Be409Put(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_DELETE_of_existing_document_Using_id_and_rev_The_document_is_deleted()
        {
            var r = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.DeleteAsync(r.Id, r.Rev).Result;

            response.Should().BeSuccessfulDelete(ClientTestData.Artists.Artist1Id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_COPY_using_srcId_The_document_is_copied()
        {
            var artistPost = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var srcArtist = SUT.GetAsync(artistPost.Id).Result;
            var newCopyId = "copyTest:1";

            var copyResponse = SUT.CopyAsync(artistPost.Id, newCopyId).Result;

            copyResponse.Id.Should().Be(newCopyId);

            var copied = SUT.GetAsync(newCopyId).Result;
            copied.Content.Should().Be(srcArtist.Content
                .Replace("\"_id\":\"" + srcArtist.Id + "\"", "\"_id\":\"" + copyResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist.Rev + "\"", "\"_rev\":\"" + copyResponse.Rev + "\""));
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_COPY_using_srcId_and_srcRev_The_document_is_copied()
        {
            var artistPost1 = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var srcArtist = SUT.GetAsync(artistPost1.Id).Result;
            const string newCopyId = "copyTest:1";

            SUT.PutAsync(srcArtist.Id, srcArtist.Content).Wait();

            var copyResponse = SUT.CopyAsync(artistPost1.Id, artistPost1.Rev, newCopyId).Result;

            copyResponse.Id.Should().Be(newCopyId);

            var copied = SUT.GetAsync(newCopyId).Result;
            copied.Content.Should().Be(srcArtist.Content
                .Replace("\"_id\":\"" + srcArtist.Id + "\"", "\"_id\":\"" + copyResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist.Rev + "\"", "\"_rev\":\"" + copyResponse.Rev + "\""));
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_REPLACE_using_srcId_The_document_is_replacing_target()
        {
            var artist1Post = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var artist2Post = SUT.PostAsync(ClientTestData.Artists.Artist2Json).Result;
            var srcArtist1 = SUT.GetAsync(artist1Post.Id).Result;

            var replaceResponse = SUT.ReplaceAsync(artist1Post.Id, artist2Post.Id, artist2Post.Rev).Result;

            replaceResponse.Id.Should().Be(artist2Post.Id);

            var replaced = SUT.GetAsync(artist2Post.Id).Result;
            replaced.Content.Should().Be(srcArtist1.Content
                .Replace("\"_id\":\"" + srcArtist1.Id + "\"", "\"_id\":\"" + replaceResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist1.Rev + "\"", "\"_rev\":\"" + replaceResponse.Rev + "\""));
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void When_REPLACE_using_srcId_and_srcRev_The_document_is_replacing_target()
        {
            var artist1Post = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var artist2Post = SUT.PostAsync(ClientTestData.Artists.Artist2Json).Result;
            var srcArtist1 = SUT.GetAsync(artist1Post.Id).Result;

            var replaceResponse = SUT.ReplaceAsync(artist1Post.Id, artist1Post.Rev, artist2Post.Id, artist2Post.Rev).Result;

            replaceResponse.Id.Should().Be(artist2Post.Id);

            var replaced = SUT.GetAsync(artist2Post.Id).Result;
            replaced.Content.Should().Be(srcArtist1.Content
                .Replace("\"_id\":\"" + srcArtist1.Id + "\"", "\"_id\":\"" + replaceResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist1.Rev + "\"", "\"_rev\":\"" + replaceResponse.Rev + "\""));
        }

        [MyFact(TestScenarios.EntitiesContext)]
        public void When_PUT_of_id_with_slash_It_will_encode_and_decode_id()
        {
            const string id = "test/1";

            var putResponse = SUT.PutAsync(id, "{\"value\":42}").Result;
            putResponse.Should().BeSuccessfulPutOfNew(id);

            var getResponse = SUT.GetAsync(id).Result;
            getResponse.Id.Should().Be(id);
        }

        [MyFact(TestScenarios.DocumentsContext)]
        public void Flow_tests()
        {
            var post1 = SUT.PostAsync(ClientTestData.Artists.Artist1Json);
            var post2 = SUT.PostAsync(ClientTestData.Artists.Artist2Json);

            post1.Result.Should().BeSuccessfulPost(ClientTestData.Artists.Artist1Id);
            post2.Result.Should().BeSuccessfulPost(ClientTestData.Artists.Artist2Id);

            var get1 = SUT.GetAsync(post1.Result.Id);
            var get2 = SUT.GetAsync(post2.Result.Id);

            get1.Result.Should().BeSuccessfulGet(post1.Result.Id, post1.Result.Rev);
            get2.Result.Should().BeSuccessfulGet(post2.Result.Id, post2.Result.Rev);

            var kv1 = DbClient.Serializer.Deserialize<IDictionary<string, dynamic>>(get1.Result.Content);
            kv1["year"] = 2000;
            var docUpd1 = DbClient.Serializer.Serialize(kv1);
            var put1 = SUT.PutAsync(get1.Result.Id, docUpd1);

            var kv2 = DbClient.Serializer.Deserialize<IDictionary<string, dynamic>>(get2.Result.Content);
            kv2["year"] = 2001;
            var docUpd2 = DbClient.Serializer.Serialize(kv2);
            var put2 = SUT.PutAsync(get2.Result.Id, docUpd2);

            put1.Result.Should().BeSuccessfulPut(get1.Result.Id);
            put2.Result.Should().BeSuccessfulPut(get2.Result.Id);

            var delete1 = SUT.DeleteAsync(put1.Result.Id, put1.Result.Rev);
            var delete2 = SUT.DeleteAsync(put2.Result.Id, put2.Result.Rev);

            delete1.Result.Should().BeSuccessfulDelete(put1.Result.Id);
            delete2.Result.Should().BeSuccessfulDelete(put2.Result.Id);
        }
    }
}