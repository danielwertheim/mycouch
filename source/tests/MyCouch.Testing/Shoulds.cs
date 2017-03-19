using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.Responses;

namespace MyCouch.Testing
{
    [DebuggerStepThrough]
    public static class Shoulds
    {
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

        public static DatabaseHeaderResponseAssertions Should(this DatabaseHeaderResponse response)
        {
            return new DatabaseHeaderResponseAssertions(response);
        }

        public static GetDatabaseResponseAssertions Should(this GetDatabaseResponse response)
        {
            return new GetDatabaseResponseAssertions(response);
        }

        public static ReplicationResponseAssertions Should(this ReplicationResponse response)
        {
            return new ReplicationResponseAssertions(response);
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

        public virtual void Be(HttpMethod method, params HttpStatusCode[] statusCodes)
        {
            Response.RequestMethod.Should().Be(method);
            Response.IsSuccess.Should().BeTrue();

            if (statusCodes.Any())
                statusCodes.Should().Contain(Response.StatusCode);
            else
                Response.StatusCode.Should().Be(HttpStatusCode.OK);

            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
        }
    }

    public class DatabaseHeaderResponseAssertions : ResponseAssertions<DatabaseHeaderResponse>
    {
        [DebuggerStepThrough]
        public DatabaseHeaderResponseAssertions(DatabaseHeaderResponse response) : base(response) { }

        public void BeAcceptedPost(string dbName)
        {
            Response.Should().Be(HttpMethod.Post, HttpStatusCode.Accepted);
            Response.IsSuccess.Should().BeTrue();
            Response.DbName.Should().NotBeNullOrEmpty();
            Response.DbName.Should().Be(dbName);
        }
    }

    public class GetDatabaseResponseAssertions : ResponseAssertions<GetDatabaseResponse>
    {
        [DebuggerStepThrough]
        public GetDatabaseResponseAssertions(GetDatabaseResponse response) : base(response) { }

        public void BeSuccessful(string dbName)
        {
            Response.Should().Be(HttpMethod.Get);
            Response.IsSuccess.Should().BeTrue();
            Response.DbName.Should().NotBeNullOrEmpty();
            Response.DbName.Should().Be(dbName);
            Response.UpdateSeq.Should().NotBeNullOrEmpty();
            Response.DataSize.Should().BeGreaterThan(0);
            Response.DiskSize.Should().BeGreaterThan(0);
            Response.DocCount.Should().BeGreaterThan(0);
            Response.DocDelCount.Should().BeGreaterThan(0);
            Response.DiskFormatVersion.Should().BeGreaterThan(0);
        }
    }

    public class ReplicationResponseAssertions : ResponseAssertions<ReplicationResponse>
    {
        [DebuggerStepThrough]
        public ReplicationResponseAssertions(ReplicationResponse response) : base(response) { }

        public void BeSuccessfulReplication(string expectedId)
        {
            Response.Should().Be(HttpMethod.Put, HttpStatusCode.Accepted, HttpStatusCode.Created);
            Response.Id.Should().Be(expectedId);
            Response.Rev.Should().NotBeNullOrWhiteSpace();
        }
    }

    public class ContentResponseAssertions : ResponseAssertions<TextResponse>
    {
        [DebuggerStepThrough]
        public ContentResponseAssertions(TextResponse response) : base(response) { }

        public void BeGetOfJson()
        {
            BeJson(HttpMethod.Get);
        }

        public void BeGetOfXml()
        {
            BeXml(HttpMethod.Get);
        }

        public void BePostOfJson()
        {
            BeJson(HttpMethod.Post);
        }

        public void BeOkJson(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            BeJson(method, statusCode, "{\"ok\":true}");
        }

        public void BeJson(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Be(method, statusCode);
            Response.ContentType.Should().Be(HttpContentTypes.Json);
            Response.Content.Should().NotBeNullOrEmpty();
        }

        public void BeJson(HttpMethod method, HttpStatusCode statusCode, string content)
        {
            Be(method, statusCode);
            Response.ContentType.Should().Be(HttpContentTypes.Json);
            Response.Content.Should().Be(content);
        }

        public void BeXml(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Be(method, statusCode);
            Response.ContentType.Should().Be(HttpContentTypes.Xml);
            Response.Content.Should().NotBeNullOrEmpty();
        }

        public void BeGetOfHtml()
        {
            BeHtml(HttpMethod.Get);
        }

        public void BePostOfHtml()
        {
            BeHtml(HttpMethod.Post);
        }

        public void BeHtml(HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Be(method, statusCode);
            Response.ContentType.Should().Contain(HttpContentTypes.Html);
            Response.Content.Should().NotBeNullOrEmpty();
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
                Response.Rows[i].Value.ShouldBeEquivalentTo(expected[i]);
        }

        private void BeSuccessful<TKey>(HttpMethod method, T[] expected, Func<ViewQueryResponse<T, TIncludedDoc>.Row, TKey> orderBy = null)
        {
            BeSuccessful(method, expected.Length);

            var actual = orderBy != null
                ? Response.Rows.OrderBy(orderBy).ToArray()
                : Response.Rows;

            for (var i = 0; i < Response.RowCount; i++)
                actual[i].Value.ShouldBeEquivalentTo(expected[i]);
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

            //https://issues.apache.org/jira/browse/COUCHDB-3331
            //Response.ETag.Should().NotBeNullOrWhiteSpace();
            if (!string.IsNullOrWhiteSpace(Response.ETag))
                Response.ETag.Should().NotContain("\"");

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

        public void BeSuccessfulGet(string id, string rev)
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
            Response.Rev.Should().Be(rev);
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
            if (initialId != null)
                Response.Id.Should().Be(initialId);

            Response.Rev.Should().NotBeNullOrEmpty();

            if (idAccessor != null)
                idAccessor(Response.Content).Should().Be(Response.Id);

            if (revAccessor != null)
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
            if (initialId != null)
                Response.Id.Should().Be(initialId);

            Response.Rev.Should().NotBeNullOrEmpty();

            if (idAccessor != null)
                idAccessor(Response.Content).Should().Be(Response.Id);

            if (revAccessor != null)
                revAccessor(Response.Content).Should().Be(Response.Rev);
        }

        public void BeSuccessfulDelete(string initialId, Func<T, string> idAccessor = null, Func<T, string> revAccessor = null)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created, HttpStatusCode.OK, };

            Response.RequestMethod.Should().Be(HttpMethod.Delete);
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
    }

    public class DocumentResponseAssertions
    {
        protected readonly DocumentResponse Response;

        [DebuggerStepThrough]
        public DocumentResponseAssertions(DocumentResponse response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(string id, string rev)
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
            Response.Rev.Should().Be(rev);
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
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
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
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
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
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created };

            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
        }

        public void BeSuccessfulDelete(string initialId)
        {
            var codes = new[] { HttpStatusCode.Accepted, HttpStatusCode.Created, HttpStatusCode.OK };

            Response.RequestMethod.Should().Be(HttpMethod.Delete);
            Response.IsSuccess.Should().BeTrue("StatusCode:" + Response.StatusCode);
            codes.Should().Contain(Response.StatusCode);
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