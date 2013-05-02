using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using FluentAssertions;

namespace MyCouch.Testing
{
    [DebuggerStepThrough]
    public static class Shoulds
    {
        public static ViewQueryResponseAssertions<T> Should<T>(this ViewQueryResponse<T> response) where T : class
        {
            return new ViewQueryResponseAssertions<T>(response);
        }

        public static EntityResponseAssertions<T> Should<T>(this EntityResponse<T> response) where T : class
        {
            return new EntityResponseAssertions<T>(response);
        }

        public static DocumentResponseAssertions Should(this JsonResponse response)
        {
            return new DocumentResponseAssertions(response);
        }
    }

    public class ViewQueryResponseAssertions<T> where T : class
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
        protected readonly JsonResponse Response;

        [DebuggerStepThrough]
        public DocumentResponseAssertions(JsonResponse response)
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
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(id);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulPost(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Post);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeTrue();
            Response.Content.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
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
            Response.IsEmpty.Should().BeTrue();
            Response.Content.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }

        public void BeSuccessfulPutOfNew(string initialId)
        {
            Response.RequestMethod.Should().Be(HttpMethod.Put);
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeTrue();
            Response.Content.Should().BeNull();
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
            Response.IsEmpty.Should().BeTrue();
            Response.Content.Should().BeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Id.Should().Be(initialId);
            Response.Rev.Should().NotBeNullOrEmpty();
        }
    }
}