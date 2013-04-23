using System;
using System.Diagnostics;
using System.Net;
using FluentAssertions;

namespace MyCouch.IntegrationTests
{
    [DebuggerStepThrough]
    internal static class Shoulds
    {
        internal static EntityResponseAssertions<T> Should<T>(this EntityResponse<T> response) where T : class
        {
            return new EntityResponseAssertions<T>(response);
        }

        internal static DocumentResponseAssertions Should(this DocumentResponse response)
        {
            return new DocumentResponseAssertions(response);
        }
    }

    internal class EntityResponseAssertions<T> where T : class
    {
        protected readonly EntityResponse<T> Response;

        [DebuggerStepThrough]
        public EntityResponseAssertions (EntityResponse<T> response)
        {
            Response = response;
        }

        internal void BeSuccessfulPost(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
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

        internal void BeSuccessfulPut(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
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

        internal void BeSuccessfulPutOfNew(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
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

        internal void BeSuccessfulDelete(string initialId, Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
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

    internal class DocumentResponseAssertions
    {
        protected readonly DocumentResponse Response;

        [DebuggerStepThrough]
        public DocumentResponseAssertions(DocumentResponse response)
        {
            Response = response;
        }

        internal void BeSuccessfulPost(string initialId)
        {
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

        internal void BeSuccessfulPut(string initialId)
        {
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

        internal void BeSuccessfulPutOfNew(string initialId)
        {
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

        internal void BeSuccessfulDelete(string initialId)
        {
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