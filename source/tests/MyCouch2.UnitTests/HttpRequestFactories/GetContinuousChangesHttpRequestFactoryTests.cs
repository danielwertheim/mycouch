using System;
using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests.HttpRequestFactories
{
    public class GetContinuousChangesHttpRequestFactoryTests : UnitTestsOf<GetContinuousChangesHttpRequestFactory>
    {
        public GetContinuousChangesHttpRequestFactoryTests()
        {
            SUT = new GetContinuousChangesHttpRequestFactory();
        }

        [Fact]
        public void When_Feed_is_assigned_Continuous_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Feed = ChangesFeed.Continuous;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?feed=continuous"));
        }

        protected virtual GetChangesRequest CreateRequest()
        {
            return new GetChangesRequest();
        }

        protected virtual void WithHttpRequestFor(GetChangesRequest request, Action<HttpRequest> a)
        {
            var req = SUT.Create(request);
            a(req);
        }
    }
}