﻿using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Testing;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    [Trait("Category", "IntegrationTests.CoreTests")]
    public class ChangesTests : IntegrationTestsOf<IChanges>
    {
        protected readonly Func<string, int> StringToNumeric = s =>
        {
            var digits = s.ToCharArray().Where(c => Char.IsDigit(c));
            var tmp = new String(digits.ToArray());
            return int.Parse(tmp);
        };

        public ChangesTests()
        {
            SUT = DbClient.Changes;
        }

        [MyFact(TestScenarios.ChangesContext)]
        public void When_getting_Normal_changes_with_included_doc_after_a_post_It_contains_the_included_doc()
        {
            var changesRequest = new GetChangesRequest { Feed = ChangesFeed.Normal };
            var changes0 = SUT.GetAsync(changesRequest).Result;
            changes0.Should().BeSuccessfulGet();

            var postOfDoc = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;

            var changesRequestWithInclude = new GetChangesRequest { Feed = ChangesFeed.Normal, IncludeDocs = true };
            var changesAfterPostOfDoc = SUT.GetAsync(changesRequestWithInclude).Result;
            VerifyChanges(changes0, changesAfterPostOfDoc, postOfDoc.Id, postOfDoc.Rev, shouldBeDeleted: false);

            var change = changesAfterPostOfDoc.Results.OrderBy(c => StringToNumeric(c.Seq)).Last();
            var postedDoc = DbClient.Documents.GetAsync(postOfDoc.Id, postOfDoc.Rev).Result.Content;

            change.IncludedDoc.Should().Be(postedDoc);
        }

        [MyFact(TestScenarios.ChangesContext)]
        public void When_getting_Normal_changes_after_each_change_It_contains_info_about_each_change()
        {
            var changesRequest = new GetChangesRequest { Feed = ChangesFeed.Normal };
            var changes0 = SUT.GetAsync(changesRequest).Result;
            changes0.Should().BeSuccessfulGet();

            var postOfDoc = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var changesAfterPostOfDoc = SUT.GetAsync(changesRequest).Result;
            VerifyChanges(changes0, changesAfterPostOfDoc, postOfDoc.Id, postOfDoc.Rev, shouldBeDeleted: false);

            var putOfDoc = DbClient.Documents.PutAsync(postOfDoc.Id, postOfDoc.Rev, ClientTestData.Artists.Artist1Json).Result;
            var changesAfterPutOfDoc = SUT.GetAsync(changesRequest).Result;
            VerifyChanges(changesAfterPostOfDoc, changesAfterPutOfDoc, putOfDoc.Id, putOfDoc.Rev, shouldBeDeleted: false);

            var deleteOfDoc = DbClient.Documents.DeleteAsync(putOfDoc.Id, putOfDoc.Rev).Result;
            var changesAfterDeleteOfDoc = SUT.GetAsync(changesRequest).Result;
            VerifyChanges(changesAfterPutOfDoc, changesAfterDeleteOfDoc, deleteOfDoc.Id, deleteOfDoc.Rev, shouldBeDeleted: true);
        }

        [MyFact(TestScenarios.ChangesContext)]
        public void When_getting_Normal_changes_after_all_changes_has_been_done_It_contains_only_info_about_the_last_change()
        {
            var changesRequest = new GetChangesRequest { Feed = ChangesFeed.Normal };
            var changes0 = SUT.GetAsync(changesRequest).Result;
            changes0.Should().BeSuccessfulGet();

            var postOfDoc = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putOfDoc = DbClient.Documents.PutAsync(postOfDoc.Id, postOfDoc.Rev, ClientTestData.Artists.Artist1Json).Result;
            var deleteOfDoc = DbClient.Documents.DeleteAsync(putOfDoc.Id, putOfDoc.Rev).Result;

            var changesAfterLastOperation = SUT.GetAsync(changesRequest).Result;
            VerifyChanges(changes0, changesAfterLastOperation, deleteOfDoc.Id, deleteOfDoc.Rev, shouldBeDeleted: true);
        }

        [MyFact(TestScenarios.ChangesContext)]
        public virtual void When_getting_Continuous_changes_via_callback_and_performing_three_changes_It_will_extract_three_changes()
        {
            var lastSequence = GetLastSequence();
            var changesRequest = new GetChangesRequest { Feed = ChangesFeed.Continuous, Since = lastSequence };
            var numOfChanges = 0;
            const int expectedNumOfChanges = 3;
            var cancellation = new CancellationTokenSource();

            var changes = SUT.GetAsync(changesRequest, data =>
            {
                Interlocked.Increment(ref numOfChanges);
                if (numOfChanges == expectedNumOfChanges)
                    cancellation.Cancel();
            }, cancellation.Token);

            var postOfDoc = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putOfDoc = DbClient.Documents.PutAsync(postOfDoc.Id, postOfDoc.Rev, ClientTestData.Artists.Artist1Json).Result;
            var deleteOfDoc = DbClient.Documents.DeleteAsync(putOfDoc.Id, putOfDoc.Rev).Result;

            var response = changes.Result;

            response.IsSuccess.Should().BeTrue();
        }

        [MyFact(TestScenarios.ChangesContext)]
        public virtual void When_getting_Continuous_changes_via_observable_and_performing_three_changes_It_will_extract_three_changes()
        {
            var lastSequence = GetLastSequence();
            var changesRequest = new GetChangesRequest { Feed = ChangesFeed.Continuous, Since = lastSequence };
            var numOfChanges = 0;
            const int expectedNumOfChanges = 3;
            var cancellation = new CancellationTokenSource();

            var changes = SUT.ObserveContinuous(changesRequest, cancellation.Token);
            changes.Subscribe(new MyObserver
            {
                InterceptOnNext = v =>
                {
                    Interlocked.Increment(ref numOfChanges);
                    if (numOfChanges >= expectedNumOfChanges)
                        cancellation.Cancel();
                }
            });

            var postOfDoc = DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json).Result;
            var putOfDoc = DbClient.Documents.PutAsync(postOfDoc.Id, postOfDoc.Rev, ClientTestData.Artists.Artist1Json).Result;
            var deleteOfDoc = DbClient.Documents.DeleteAsync(putOfDoc.Id, putOfDoc.Rev).Result;

            SpinWait.SpinUntil(() => cancellation.IsCancellationRequested);
            numOfChanges.Should().Be(expectedNumOfChanges);
        }

        private string GetLastSequence()
        {
            var changes = SUT.GetAsync(new GetChangesRequest { Feed = ChangesFeed.Normal }).Result;

            return changes.LastSeq;
        }

        protected virtual void VerifyChanges<T>(ChangesResponse<T> previous, ChangesResponse<T> current, string expectedId, string expectedRev, bool shouldBeDeleted)
        {
            current.Should().BeSuccessfulGet();

            var lastChange = current.Results.Select(c => new { NumericSeq = StringToNumeric(c.Seq), Change = c }).OrderBy(i => i.NumericSeq).Last();
            lastChange.Change.Id.Should().Be(expectedId);
            lastChange.Change.Changes.Single().Rev.Should().Be(expectedRev);
            lastChange.Change.Deleted.Should().Be(shouldBeDeleted);
        }
    }
}