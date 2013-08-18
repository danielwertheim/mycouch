using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class DocumentsTests : IntegrationTestsOf<IDocuments>
    {
        public DocumentsTests()
        {
            SUT = Client.Documents;
        }

        [Fact]
        public void When_exists_of_non_existing_document_The_response_is_empty()
        {
            var response = SUT.Exists("fooId");

            response.Should().BeHead404("fooId");
        }

        [Fact]
        public void When_exists_using_not_matching_rev_The_response_is_empty()
        {
            var postResponse = SUT.Post(TestData.Artists.Artist1Json);

            var response = SUT.Exists(postResponse.Id, "1-795258d03c3bdb58fffc409e153c5d45");

            response.Should().BeHead404(postResponse.Id);
        }

        [Fact]
        public void When_exists_using_matching_id_The_response_is_ok()
        {
            var postResponse = SUT.Post(TestData.Artists.Artist1Json);

            var response = SUT.Exists(postResponse.Id);

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [Fact]
        public void When_exists_using_matching_id_and_rev_The_response_is_ok()
        {
            var postResponse = SUT.Post(TestData.Artists.Artist1Json);

            var response = SUT.Exists(postResponse.Id, postResponse.Rev);

            response.Should().BeHead200(postResponse.Id, postResponse.Rev);
        }

        [Fact]
        public void When_post_of_new_document_The_document_is_persisted()
        {
            var response = SUT.Post(TestData.Artists.Artist1Json);

            response.Should().BeSuccessfulPost(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_put_of_new_document_The_document_is_replaced()
        {
            var response = SUT.Put(TestData.Artists.Artist1Id, TestData.Artists.Artist1Json);

            response.Should().BeSuccessfulPutOfNew(TestData.Artists.Artist1Id);
        }
        
        [Fact]
        public void When_put_of_existing_document_The_document_is_replaced()
        {
            var postResponse = SUT.Post(TestData.Artists.Artist1Json);
            var getResponse = SUT.Get(postResponse.Id);

            var response = SUT.Put(getResponse.Id, getResponse.Content);

            response.Should().BeSuccessfulPut(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_put_of_existing_document_Using_wrong_rev_A_conflict_is_detected()
        {
            var postResponse = SUT.Post(TestData.Artists.Artist1Json);

            var response = SUT.Put(postResponse.Id, "2-179d36174ee192594c63b8e8d8f09345", TestData.Artists.Artist1Json);

            response.Should().Be409Put(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_delete_of_existing_document_Using_id_and_rev_The_document_is_deleted()
        {
            var r = SUT.Post(TestData.Artists.Artist1Json);

            var response = SUT.Delete(r.Id, r.Rev);

            response.Should().BeSuccessfulDelete(TestData.Artists.Artist1Id);
        }

        [Fact]
        public void When_copying_using_srcId_The_document_is_copied()
        {
            var artistPost = SUT.Post(TestData.Artists.Artist1Json);
            var srcArtist = SUT.Get(artistPost.Id);
            var newCopyId = "copyTest:1";

            var copyResponse = SUT.Copy(artistPost.Id, newCopyId);

            copyResponse.Id.Should().Be(newCopyId);

            var copied = SUT.Get(newCopyId);
            copied.Content.Should().Be(srcArtist.Content
                .Replace("\"_id\":\"" + srcArtist.Id + "\"", "\"_id\":\"" + copyResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist.Rev + "\"", "\"_rev\":\"" + copyResponse.Rev + "\""));
        }

        [Fact]
        public void When_copying_using_srcId_and_srcRev_The_document_is_copied()
        {
            var artistPost1 = SUT.Post(TestData.Artists.Artist1Json);
            var srcArtist = SUT.Get(artistPost1.Id);
            var artistPost2 = SUT.Put(srcArtist.Id, srcArtist.Content);
            var newCopyId = "copyTest:1";

            var copyResponse = SUT.Copy(artistPost1.Id, artistPost1.Rev, newCopyId);

            copyResponse.Id.Should().Be(newCopyId);

            var copied = SUT.Get(newCopyId);
            copied.Content.Should().Be(srcArtist.Content
                .Replace("\"_id\":\"" + srcArtist.Id + "\"", "\"_id\":\"" + copyResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist.Rev + "\"", "\"_rev\":\"" + copyResponse.Rev + "\""));
        }

        [Fact]
        public void When_replacing_using_srcId_The_document_is_replacing_target()
        {
            var artist1Post = SUT.Post(TestData.Artists.Artist1Json);
            var artist2Post = SUT.Post(TestData.Artists.Artist2Json);
            var srcArtist1 = SUT.Get(artist1Post.Id);

            var replaceResponse = SUT.Replace(artist1Post.Id, artist2Post.Id, artist2Post.Rev);

            replaceResponse.Id.Should().Be(artist2Post.Id);

            var replaced = SUT.Get(artist2Post.Id);
            replaced.Content.Should().Be(srcArtist1.Content
                .Replace("\"_id\":\"" + srcArtist1.Id + "\"", "\"_id\":\"" + replaceResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist1.Rev + "\"", "\"_rev\":\"" + replaceResponse.Rev + "\""));
        }

        [Fact]
        public void When_replacing_using_srcId_and_srcRev_The_document_is_replacing_target()
        {
            var artist1Post = SUT.Post(TestData.Artists.Artist1Json);
            var artist2Post = SUT.Post(TestData.Artists.Artist2Json);
            var srcArtist1 = SUT.Get(artist1Post.Id);

            var replaceResponse = SUT.Replace(artist1Post.Id, artist1Post.Rev, artist2Post.Id, artist2Post.Rev);

            replaceResponse.Id.Should().Be(artist2Post.Id);

            var replaced = SUT.Get(artist2Post.Id);
            replaced.Content.Should().Be(srcArtist1.Content
                .Replace("\"_id\":\"" + srcArtist1.Id + "\"", "\"_id\":\"" + replaceResponse.Id + "\"")
                .Replace("\"_rev\":\"" + srcArtist1.Rev + "\"", "\"_rev\":\"" + replaceResponse.Rev + "\""));
        }

        [Fact]
        public void Flow_tests()
        {
            var post1 = SUT.PostAsync(TestData.Artists.Artist1Json);
            var post2 = SUT.Post(TestData.Artists.Artist2Json);

            post1.Result.Should().BeSuccessfulPost(TestData.Artists.Artist1Id);
            post2.Should().BeSuccessfulPost(TestData.Artists.Artist2Id);

            var get1 = SUT.GetAsync(post1.Result.Id);
            var get2 = SUT.Get(post2.Id);

            get1.Result.Should().BeSuccessfulGet(post1.Result.Id);
            get2.Should().BeSuccessfulGet(post2.Id);

            var kv1 = Client.Serializer.Deserialize<IDictionary<string, dynamic>>(get1.Result.Content);
            kv1["year"] = 2000;
            var docUpd1 = Client.Serializer.Serialize(kv1);

            var put1 = SUT.PutAsync(get1.Result.Id, docUpd1);

            var kv2 = Client.Serializer.Deserialize<IDictionary<string, dynamic>>(get2.Content);
            kv2["year"] = 2001;
            var docUpd2 = Client.Serializer.Serialize(kv2);
            var put2 = SUT.Put(get2.Id, docUpd2);

            put1.Result.Should().BeSuccessfulPut(get1.Result.Id);
            put2.Should().BeSuccessfulPut(get2.Id);

            var delete1 = SUT.DeleteAsync(put1.Result.Id, put1.Result.Rev);
            var delete2 = SUT.Delete(put2.Id, put2.Rev);

            delete1.Result.Should().BeSuccessfulDelete(put1.Result.Id);
            delete2.Should().BeSuccessfulDelete(put2.Id);
        }
    }
}