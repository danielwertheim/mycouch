using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class DocumentsTests : ClientTestsOf<IDocuments>
    {
        protected override void OnTestInit()
        {
            SUT = Client.Documents;
        }

        [Fact]
        public void When_exists_of_non_existing_document_The_response_is_empty()
        {
            var response = SUT.ExistsAsync("fooId").Result;

            response.Should().BeHead404("fooId");
        }

        [Fact]
        public void When_exists_using_not_matching_rev_The_response_is_empty()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.ExistsAsync(postResponse.Id, "1-795258d03c3bdb58fffc409e153c5d45").Result;

            response.Should().BeHead404(postResponse.Id);
        }

        [Fact]
        public void When_exists_using_matching_id_The_response_is_ok()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.ExistsAsync(postResponse.Id).Result;

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [Fact]
        public void When_exists_using_matching_id_and_rev_The_response_is_ok()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.ExistsAsync(postResponse.Id, postResponse.Rev).Result;

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [Fact]
        public void When_post_of_new_document_The_document_is_persisted()
        {
            var response = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            response.Should().BeSuccessfulPost(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_put_of_new_document_The_document_is_replaced()
        {
            var response = SUT.PutAsync(ClientTestData.Artists.Artist1Id, ClientTestData.Artists.Artist1Json).Result;

            response.Should().BeSuccessfulPutOfNew(ClientTestData.Artists.Artist1Id);
        }
        
        [Fact]
        public void When_put_of_existing_document_The_document_is_replaced()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var getResponse = SUT.GetAsync(postResponse.Id).Result;

            var response = SUT.PutAsync(getResponse.Id, getResponse.Content).Result;

            response.Should().BeSuccessfulPut(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_put_of_existing_document_Using_wrong_rev_A_conflict_is_detected()
        {
            var postResponse = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.PutAsync(postResponse.Id, "2-179d36174ee192594c63b8e8d8f09345", ClientTestData.Artists.Artist1Json).Result;

            response.Should().Be409Put(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_delete_of_existing_document_Using_id_and_rev_The_document_is_deleted()
        {
            var r = SUT.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var response = SUT.DeleteAsync(r.Id, r.Rev).Result;

            response.Should().BeSuccessfulDelete(ClientTestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_copying_using_srcId_The_document_is_copied()
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

        [Fact]
        public void When_copying_using_srcId_and_srcRev_The_document_is_copied()
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

        [Fact]
        public void When_replacing_using_srcId_The_document_is_replacing_target()
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

        [Fact]
        public void When_replacing_using_srcId_and_srcRev_The_document_is_replacing_target()
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

        [Fact]
        public void Flow_tests()
        {
            var post1 = SUT.PostAsync(ClientTestData.Artists.Artist1Json);
            var post2 = SUT.PostAsync(ClientTestData.Artists.Artist2Json);

            post1.Result.Should().BeSuccessfulPost(ClientTestData.Artists.Artist1Id);
            post2.Result.Should().BeSuccessfulPost(ClientTestData.Artists.Artist2Id);

            var get1 = SUT.GetAsync(post1.Result.Id);
            var get2 = SUT.GetAsync(post2.Result.Id);

            get1.Result.Should().BeSuccessfulGet(post1.Result.Id);
            get2.Result.Should().BeSuccessfulGet(post2.Result.Id);

            var kv1 = Client.Serializer.Deserialize<IDictionary<string, dynamic>>(get1.Result.Content);
            kv1["year"] = 2000;
            var docUpd1 = Client.Serializer.Serialize(kv1);
            var put1 = SUT.PutAsync(get1.Result.Id, docUpd1);

            var kv2 = Client.Serializer.Deserialize<IDictionary<string, dynamic>>(get2.Result.Content);
            kv2["year"] = 2001;
            var docUpd2 = Client.Serializer.Serialize(kv2);
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