using System.Linq;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Testing;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class ChangesTests : ClientTestsOf<IChanges>
    {
        protected override void OnTestInit()
        {
            SUT = Client.Changes;
        }

        [Fact]
        public void When_getting_Normal_changes_with_included_doc_after_a_post_It_contains_the_included_doc()
        {
            var changes0 = SUT.GetAsync(ChangesFeed.Normal).Result;
            changes0.Should().BeSuccessfulGet();

            var postOfDoc = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var getChangesRequest = new GetChangesRequest { Feed = ChangesFeed.Normal, IncludeDocs = true };
            var changesAfterPostOfDoc = SUT.GetAsync(getChangesRequest).Result;
            VerifyChanges(changes0, changesAfterPostOfDoc, postOfDoc.Id, postOfDoc.Rev, shouldBeDeleted: false);

            var change = changesAfterPostOfDoc.Results.OrderBy(c => c.Seq).Last();
            var postedDoc = Client.Documents.GetAsync(postOfDoc.Id, postOfDoc.Rev).Result.Content;

            change.IncludedDoc.Should().Be(postedDoc);
        }

        [Fact]
        public void When_getting_Normal_changes_after_each_change_It_contains_info_about_each_change()
        {
            var changes0 = SUT.GetAsync(ChangesFeed.Normal).Result;
            changes0.Should().BeSuccessfulGet();

            var postOfDoc = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var changesAfterPostOfDoc = SUT.GetAsync(ChangesFeed.Normal).Result;
            VerifyChanges(changes0, changesAfterPostOfDoc, postOfDoc.Id, postOfDoc.Rev, shouldBeDeleted: false);

            var putOfDoc = Client.Documents.PutAsync(postOfDoc.Id, postOfDoc.Rev, ClientTestData.Artists.Artist1Json).Result;
            var changesAfterPutOfDoc = SUT.GetAsync(ChangesFeed.Normal).Result;
            VerifyChanges(changesAfterPostOfDoc, changesAfterPutOfDoc, putOfDoc.Id, putOfDoc.Rev, shouldBeDeleted: false);

            var deleteOfDoc = Client.Documents.DeleteAsync(putOfDoc.Id, putOfDoc.Rev).Result;
            var changesAfterDeleteOfDoc = SUT.GetAsync(ChangesFeed.Normal).Result;
            VerifyChanges(changesAfterPutOfDoc, changesAfterDeleteOfDoc, deleteOfDoc.Id, deleteOfDoc.Rev, shouldBeDeleted: true);
        }

        [Fact]
        public void When_getting_Normal_changes_after_all_changes_has_been_done_It_contains_only_info_about_the_last_change()
        {
            var changes0 = SUT.GetAsync(ChangesFeed.Normal).Result;
            changes0.Should().BeSuccessfulGet();

            var postOfDoc = Client.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putOfDoc = Client.Documents.PutAsync(postOfDoc.Id, postOfDoc.Rev, ClientTestData.Artists.Artist1Json).Result;
            var deleteOfDoc = Client.Documents.DeleteAsync(putOfDoc.Id, putOfDoc.Rev).Result;

            var changesAfterLastOperation = SUT.GetAsync(ChangesFeed.Normal).Result;
            VerifyChanges(changes0, changesAfterLastOperation, deleteOfDoc.Id, deleteOfDoc.Rev, shouldBeDeleted: true, numOfChangesPerformed: 3);
        }

        protected virtual void VerifyChanges<T>(ChangesResponse<T> previous, ChangesResponse<T> current, string expectedId, string expectedRev, bool shouldBeDeleted, int numOfChangesPerformed = 1)
        {
            current.Should().BeSuccessfulGet(previous.LastSeq + numOfChangesPerformed);

            var change = current.Results.Single(c => c.Seq > previous.LastSeq);
            change.Id.Should().Be(expectedId);
            change.Changes.Single().Rev.Should().Be(expectedRev);
            change.Deleted.Should().Be(shouldBeDeleted);
        }
    }
}