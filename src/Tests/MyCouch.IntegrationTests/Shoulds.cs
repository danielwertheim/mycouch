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
    }

    internal class EntityResponseAssertions<T> where T : class
    {
        protected readonly EntityResponse<T> Response;

        [DebuggerStepThrough]
        public EntityResponseAssertions (EntityResponse<T> response)
        {
            Response = response;
        }

        internal void BeSuccessfulPost(Func<T, string> idAccessor, Func<T, string> revAccessor)
        {
            Response.IsSuccess.Should().BeTrue();
            Response.StatusCode.Should().Be(HttpStatusCode.Created);
            Response.Error.Should().BeNull();
            Response.Reason.Should().BeNull();
            Response.IsEmpty.Should().BeFalse();
            Response.Entity.Should().NotBeNull();
            Response.Id.Should().NotBeNullOrEmpty();
            Response.Rev.Should().NotBeNullOrEmpty();
            
            idAccessor(Response.Entity).Should().Be(Response.Id);
            revAccessor(Response.Entity).Should().Be(Response.Rev);
        }
    }
}