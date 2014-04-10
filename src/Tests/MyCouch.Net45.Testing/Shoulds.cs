using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Cloudant.Responses;
using MyCouch.Net;
using MyCouch.Responses;

namespace MyCouch.Testing
{
    [DebuggerStepThrough]
    public static class Shoulds
    {
        public static ObjectAssertions<T> ShouldBe<T>(this T item)
        {
            return new ObjectAssertions<T>(item);
        }

        public static ContentResponseAssertions Should(this TextResponse response)
        {
            return new ContentResponseAssertions(response);
        }

        public static SearchIndexResponseAssertions Should(this SearchIndexResponse response)
        {
            return new SearchIndexResponseAssertions(response);
        }

        public static SearchIndexResponseAssertions<TIncludedDoc> Should<TIncludedDoc>(this SearchIndexResponse<TIncludedDoc> response)
        {
            return new SearchIndexResponseAssertions<TIncludedDoc>(response);
        }

        public static ViewQueryResponseAssertions Should(this ViewQueryResponse response)
        {
            return new ViewQueryResponseAssertions(response);
        }

        public static ViewQueryResponseAssertions<T> Should<T>(this ViewQueryResponse<T> response)
        {
            return new ViewQueryResponseAssertions<T>(response);
        }

        public static ViewQueryResponseAssertions<T, TIncludedDoc> Should<T, TIncludedDoc>(this ViewQueryResponse<T, TIncludedDoc> response)
        {
            return new ViewQueryResponseAssertions<T, TIncludedDoc>(response);
        }

        public static DocumentResponseAssertions Should(this DocumentResponse response)
        {
            return new DocumentResponseAssertions(response);
        }

        public static EntityResponseAssertions<T> Should<T>(this EntityResponse<T> response) where T : class
        {
            return new EntityResponseAssertions<T>(response);
        }

        public static AttachmentResponseAssertions Should(this AttachmentResponse response)
        {
            return new AttachmentResponseAssertions(response);
        }

        public static DocumentHeaderResponseAssertions Should(this DocumentHeaderResponse response)
        {
            return new DocumentHeaderResponseAssertions(response);
        }

        public static ChangesResponseAssertions<T> Should<T>(this ChangesResponse<T> response)
        {
            return new ChangesResponseAssertions<T>(response);
        }

        public static ReplicationResponseAssertions Should(this ReplicationResponse response)
        {
            return new ReplicationResponseAssertions(response);
        }
    }

    public class ObjectAssertions<T>
    {
        private readonly T _item;

        public ObjectAssertions(T item)
        {
            _item = item;
        }

        public ObjectAssertions<T> ValueEqual(T expected)
        {
            CustomAsserts.AreValueEqual(expected, _item);

            return this;
        }
    }

    public abstract class ResponseAssertions<T> where T : Response
    {
        protected readonly T Response;

        [DebuggerStepThrough]
        protected ResponseAssertions(T response)
        {
            Response = response;
        }

        public virtual void Be(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Response.RequestMethod.Should().Be(method);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(statusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
        }
    }

    public class ReplicationResponseAssertions : ResponseAssertions<ReplicationResponse>
    {
        [DebuggerStepThrough]
        public ReplicationResponseAssertions(ReplicationResponse response) : base(response) { }

        public void BeSuccessfulButEmptyReplication()
        {
            Response.Should().Be(HttpMethod.Post);
            Response.NoChanges.Should().BeTrue();
        }

        public void BeSuccessfulContinousReplication()
        {
            Response.Should().Be(HttpMethod.Post, HttpStatusCode.Accepted);
            Response.LocalId.Should().NotBeNullOrWhiteSpace();
        }

        public void BeSuccessfulCancelledContinousReplication(string localId)
        {
            Response.Should().Be(HttpMethod.Post, HttpStatusCode.OK);
            Response.LocalId.Should().NotBeNullOrWhiteSpace();
            Response.LocalId.Should().Be(localId);
        }

        public void BeSuccessfulNonEmptyReplication(int expectedNumOfReplDocs)
        {
            Response.Should().Be(HttpMethod.Post);
            Response.NoChanges.Should().BeFalse();
            Response.ReplicationIdVersion.Should().BeGreaterThan(0);
            Response.History.Should().NotBeEmpty();
            Response.SessionId.Should().NotBeNullOrWhiteSpace();
            Response.History.First().SessionId.Should().Be(Response.SessionId);
            Response.History.First().EndLastSeq.Should().Be(Response.SourceLastSeq);

            foreach (var history in Response.History)
            {
                history.SessionId.Should().NotBeNullOrWhiteSpace();
                history.StartTime.Should().BeAfter(DateTime.MinValue);
                history.EndTime.Should().BeAfter(DateTime.MinValue);
                history.EndLastSeq.Should().NotBeNullOrWhiteSpace();
                history.StartLastSeq.Should().NotBeNullOrWhiteSpace();
                history.RecordedSeq.Should().NotBeNullOrWhiteSpace();

                history.MissingFound.Should().Be(expectedNumOfReplDocs);
                history.MissingChecked.Should().Be(expectedNumOfReplDocs);
                history.DocsRead.Should().Be(expectedNumOfReplDocs);
                history.DocsWritten.Should().Be(expectedNumOfReplDocs);
                history.DocWriteFailures.Should().Be(0);
            }
        }
    }

    public class ContentResponseAssertions : ResponseAssertions<TextResponse>
    {
        [DebuggerStepThrough]
        public ContentResponseAssertions(TextResponse response) : base(response) { }

        public void BeJson(HttpMethod method, HttpStatusCode statusCode, string content)
        {
            Be(method, statusCode);
            Response.ContentType.Should().Be(HttpContentTypes.Json);
            Response.Content.Should().Be(content);
        }

        public void BeAnyJson(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Be(method, statusCode);
            Response.ContentType.Should().Be(HttpContentTypes.Json);
            Response.Content.Should().NotBeNullOrWhiteSpace();
        }

        public void BeOkJson(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            BeJson(method, statusCode, "{\"ok\":true}");
        }
    }

    public class SearchIndexResponseAssertions : SearchIndexResponseAssertions<string>
    {
        [DebuggerStepThrough]
        public SearchIndexResponseAssertions(SearchIndexResponse response) : base(response) { }
    }

    public class SearchIndexResponseAssertions<TIncludedDoc>
    {
        protected readonly SearchIndexResponse<TIncludedDoc> Response;

        [DebuggerStepThrough]
        public SearchIndexResponseAssertions(SearchIndexResponse<TIncludedDoc> response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(int numOfRows)
        {
            BeSuccessful(HttpMethod.Get, numOfRows);
        }

        private void BeSuccessful(HttpMethod method, int numOfRows)
        {
            Response.RequestMethod.Should().Be(method);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();

            if (numOfRows > 0)
            {
                Response.Rows.Should().NotBeNull();
                Response.RowCount.Should().Be(numOfRows);
            }
        }
    }

    public class ViewQueryResponseAssertions : ViewQueryResponseAssertions<string, string>
    {
        [DebuggerStepThrough]
        public ViewQueryResponseAssertions(ViewQueryResponse response)
            : base(response)
        {
        }
    }

    public class ViewQueryResponseAssertions<T> : ViewQueryResponseAssertions<T, string>
    {
        [DebuggerStepThrough]
        public ViewQueryResponseAssertions(ViewQueryResponse<T> response)
            : base(response)
        {
        }
    }

    public class ViewQueryResponseAssertions<T, TIncludedDoc>
    {
        protected readonly ViewQueryResponse<T, TIncludedDoc> Response;

        [DebuggerStepThrough]
        public ViewQueryResponseAssertions(ViewQueryResponse<T, TIncludedDoc> response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(T[] expected)
        {
            BeSuccessful(HttpMethod.Get, expected);
        }

        public void BeSuccessfulGet<TKey>(T[] expected, Func<ViewQueryResponse<T, TIncludedDoc>.Row, TKey> orderBy)
        {
            BeSuccessful(HttpMethod.Get, expected, orderBy);
        }

        public void BeSuccessfulPost(T[] expected)
        {
            BeSuccessful(HttpMethod.Post, expected);
        }

        private void BeSuccessful(HttpMethod method, T[] expected)
        {
            BeSuccessful(method, expected.Length);
            for (var i = 0; i < Response.RowCount; i++)
                CustomAsserts.AreValueEqual(expected[i], Response.Rows[i].Value);
        }

        private void BeSuccessful<TKey>(HttpMethod method, T[] expected, Func<ViewQueryResponse<T, TIncludedDoc>.Row, TKey> orderBy = null)
        {
            BeSuccessful(method, expected.Length);

            var actual = orderBy != null
                ? Response.Rows.OrderBy(orderBy).ToArray()
                : Response.Rows;

            for (var i = 0; i < Response.RowCount; i++)
                CustomAsserts.AreValueEqual(expected[i], actual[i].Value);
        }

        public void BeSuccessfulPost(int numOfRows)
        {
            BeSuccessful(HttpMethod.Post, numOfRows);
        }

        public void BeSuccessfulGet(int numOfRows)
        {
            BeSuccessful(HttpMethod.Get, numOfRows);
        }

        private void BeSuccessful(HttpMethod method, int numOfRows)
        {
            Response.RequestMethod.Should().Be(method);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();

            if (numOfRows > 0)
            {
                Response.Rows.Should().NotBeNull();
                Response.RowCount.Should().Be(numOfRows);
            }
        }
    }

    public class EntityResponseAssertions<T> where T : class
    {
        protected readonly EntityResponse<T> Response;

        [DebuggerStepThrough]
        public EntityResponseAssertions(EntityResponse<T> response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(string id)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(id);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulPost(string initialId = null, Func<T, string> idAccessor = null, Func<T, string> revAccessor = null)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNull();

            Response.Id.Should().NotBeNullOrEmpty();
            if(initialId != null)
                Response.Id.Should().Be(initialId);

            Response.Rev.Should().NotBeNullOrEmpty();

            if(idAccessor != null)
                idAccessor(Response.Content).Should().Be(Response.Id);

            if(revAccessor != null)
                revAccessor(Response.Content).Should().Be(Response.Rev);
        }

        public void BeSuccessfulPut(string initialId, Func<T, string> idAccessor = null, Func<T, string> revAccessor = null)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();

            if (idAccessor != null)
                idAccessor(Response.Content).Should().Be(Response.Id);

            if (revAccessor != null)
                revAccessor(Response.Content).Should().Be(Response.Rev);
        }

        public void Be409Put(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeFalse("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            Response.Error.Should().Be("conflict");
            Response.Reason.Should().Be("Document update conflict.");
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().BeNull();
        }

        public void BeSuccessfulPutOfNew(string initialId = null, Func<T, string> idAccessor = null, Func<T, string> revAccessor = null)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNull();

            Response.Id.Should().NotBeNullOrEmpty();
            if(initialId != null)
                Response.Id.Should().Be(initialId);

            Response.Rev.Should().NotBeNullOrEmpty();

            if (idAccessor != null)
                idAccessor(Response.Content).Should().Be(Response.Id);

            if (revAccessor != null)
                revAccessor(Response.Content).Should().Be(Response.Rev);
        }

        public void BeSuccessfulDelete(string initialId, Func<T, string> idAccessor = null, Func<T, string> revAccessor = null)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Delete);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();

            if (idAccessor != null)
                idAccessor(Response.Content).Should().Be(Response.Id);

            if (revAccessor != null)
                revAccessor(Response.Content).Should().Be(Response.Rev);
        }
    }

    public class DocumentResponseAssertions
    {
        protected readonly DocumentResponse Response;

        [DebuggerStepThrough]
        public DocumentResponseAssertions(DocumentResponse response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(string id)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNullOrEmpty();
            Response.Id.Should().NotBeNullOrEmpty().And.Be(id);
            Response.Rev.Should().NotBeNullOrEmpty();
        }
    }

    public class AttachmentResponseAssertions
    {
        protected readonly AttachmentResponse Response;

        [DebuggerStepThrough]
        public AttachmentResponseAssertions(AttachmentResponse response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(string docId, string attachmentName)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Content.Should().NotBeNull().And.NotBeEmpty();
            Response.Id.Should().NotBeNullOrEmpty().And.Be(docId);
            Response.Rev.Should().NotBeNullOrEmpty();
            Response.Name.Should().NotBeNullOrEmpty().And.Be(attachmentName);
        }
    }

    public class DocumentHeaderResponseAssertions
    {
        protected readonly DocumentHeaderResponse Response;

        [DebuggerStepThrough]
        public DocumentHeaderResponseAssertions(DocumentHeaderResponse response)
        {
            Response = response;
        }

        public void BeHead404(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Head);
            Response.IsSuccess.Should().BeFalse("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().BeNull();
        }

        public void BeHead200(string id, string rev)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Head);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(id);
            Response.Rev.Should().Be(rev);
        }

        public void BeSuccessfulPost(string initialId)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);

            if (Response.StatusCode == HttpStatusCode.Created)
                Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulBatchPost(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
        }

        public void BeSuccessfulPut(string initialId)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulBatchPut(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
        }

        public void Be409Put(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeFalse("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            Response.Error.Should().Be("conflict");
            Response.Reason.Should().Be("Document update conflict.");
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().BeNull();
        }

        public void BeSuccessfulPutOfNew(string initialId)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            if (Response.StatusCode == HttpStatusCode.Created)
                Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulBatchPutOfNew(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
        }

        public void BeSuccessfulDelete(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Delete);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }
    }

    public class ChangesResponseAssertions<T>
    {
        protected readonly ChangesResponse<T> Response;

        [DebuggerStepThrough]
        public ChangesResponseAssertions(ChangesResponse<T> response)
        {
            Response = response;
        }

        public void BeSuccessfulGet()
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.LastSeq.Should().NotBeNullOrEmpty();
        }
    }
}