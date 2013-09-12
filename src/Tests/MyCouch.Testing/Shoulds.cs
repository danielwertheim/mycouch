using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Responses;

namespace MyCouch.Testing
{
    [DebuggerStepThrough]
    public static class Shoulds
    {
        public static ViewQueryResponseAssertions Should(this JsonViewQueryResponse response)
        {
            return new ViewQueryResponseAssertions(response);
        }

        public static ViewQueryResponseAssertions<T> Should<T>(this ViewQueryResponse<T> response)
        {
            return new ViewQueryResponseAssertions<T>(response);
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
    }

    public class ViewQueryResponseAssertions
    {
        protected readonly JsonViewQueryResponse Response;

        [DebuggerStepThrough]
        public ViewQueryResponseAssertions(JsonViewQueryResponse response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(string[] expected)
        {
            BeSuccessfulGet(expected.Length);
            for (var i = 0; i < Response.RowCount; i++)
                CustomAsserts.AreValueEqual(expected[i], Response.Rows[i].Value);
        }

        public void BeSuccessfulGet(int numOfRows)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
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

    public class ViewQueryResponseAssertions<T>
    {
        protected readonly ViewQueryResponse<T> Response;

        [DebuggerStepThrough]
        public ViewQueryResponseAssertions(ViewQueryResponse<T> response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(T[] expected)
        {
            BeSuccessfulGet(expected.Length);
            for (var i = 0; i < Response.RowCount; i++)
                CustomAsserts.AreValueEqual(expected[i], Response.Rows[i].Value);
        }

        public void BeSuccessfulGet(int numOfRows)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
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

    public class EntityResponseAssertions<T> where T : class
    {
        protected readonly EntityResponse<T> Response;

        [DebuggerStepThrough]
        public EntityResponseAssertions (EntityResponse<T> response)
        {
            Response = response;
        }

        public void BeSuccessfulGet(string id)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Get);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Entity.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(id);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulPost(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Entity.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();

            idAccessor(Response.Entity).Should().Be(Response.Id);
            revAccessor(Response.Entity).Should().Be(Response.Rev);
        }

        public void BeSuccessfulPut(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Entity.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();

            idAccessor(Response.Entity).Should().Be(Response.Id);
            revAccessor(Response.Entity).Should().Be(Response.Rev);
        }

        public void Be409Put(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeFalse();
            Response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            Response.Error.Should().Be("conflict");
            Response.Reason.Should().Be("Document update conflict.");
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().BeNull();
        }

        public void BeSuccessfulPutOfNew(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Entity.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();

            idAccessor(Response.Entity).Should().Be(Response.Id);
            revAccessor(Response.Entity).Should().Be(Response.Rev);
        }

        public void BeSuccessfulDelete(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Delete);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Entity.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();

            idAccessor(Response.Entity).Should().Be(Response.Id);
            revAccessor(Response.Entity).Should().Be(Response.Rev);
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
            Response.IsSuccess.Should().BeTrue();
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
            Response.IsSuccess.Should().BeTrue();
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
            Response.IsSuccess.Should().BeFalse();
            Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().BeNull();
        }

        public void BeHead200(string id, string rev)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Head);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(id);
            Response.Rev.Should().Be(rev);
        }

        public void BeSuccessfulPost(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulPut(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void Be409Put(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeFalse();
            Response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            Response.Error.Should().Be("conflict");
            Response.Reason.Should().Be("Document update conflict.");
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().BeNull();
        }

        public void BeSuccessfulPutOfNew(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulDelete(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Delete);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }
    }
}