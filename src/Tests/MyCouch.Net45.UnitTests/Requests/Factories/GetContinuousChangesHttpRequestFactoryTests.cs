using System;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class GetContinuousChangesHttpRequestFactoryTests : UnitTestsOf<GetContinuousChangesHttpRequestFactory>
    {
        public GetContinuousChangesHttpRequestFactoryTests()
        {
            var cnFake = new DbClientConnectionFake(new Uri("https://cdb.foo.com:5984/mydb"), "mydb");

            SUT = new GetContinuousChangesHttpRequestFactory(cnFake);
        }

        [Fact]
        public void When_Feed_is_assigned_Continuous_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Feed = ChangesFeed.Continuous;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?feed=continuous"));
        }

        protected virtual GetChangesRequest CreateRequest()
        {
            return new GetChangesRequest();
        }

        protected virtual void WithHttpRequestFor(GetChangesRequest request, Action<HttpRequestMessage> a)
        {
            using (var req = SUT.Create(request))
                a(req);
        }
    }
}