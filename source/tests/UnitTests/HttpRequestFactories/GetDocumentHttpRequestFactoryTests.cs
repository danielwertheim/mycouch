using System;
using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Testing;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class GetDocumentHttpRequestFactoryTests : UnitTestsOf<GetDocumentHttpRequestFactory>
    {
        protected const string FakeId = "someId";
        
        public GetDocumentHttpRequestFactoryTests()
        {
            SUT = new GetDocumentHttpRequestFactory();
        }

        [Fact]
        public void When_configured_with_id_rev_and_other_param_It_yields_correct_url()
        {
            const string fakeRev = "someRev";
            var request = CreateRequest(rev: fakeRev);
            request.Conflicts = true;

            WithHttpRequestFor(
                request,
                req =>
                {
                    req.RelativeUrl.ToTestUriFromRelative().AbsolutePath.Should().EndWith("/" + FakeId);
                    req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?rev=" + fakeRev + "&conflicts=true");
                });
        }

        [Fact]
        public void When_configured_with_id_It_yields_url_with_id()
        {
            var request = CreateRequest();

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().AbsolutePath.Should().EndWith("/" + FakeId));
        }

        [Fact]
        public void When_configured_with_rev_It_yields_url_with_rev()
        {
            const string fakeRev = "someRev";
            var request = CreateRequest(rev: fakeRev);

            WithHttpRequestFor(
                request,
                req =>
                {
                    req.RelativeUrl.ToTestUriFromRelative().AbsolutePath.Should().EndWith("/" + FakeId);
                    req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?rev=" + fakeRev);
                });
        }

        [Fact]
        public void When_configured_with_conflicts_It_yields_url_with_conflicts()
        {
            var request = CreateRequest();
            request.Conflicts = true;

            WithHttpRequestFor(
                request,
                req =>
                {
                    req.RelativeUrl.ToTestUriFromRelative().AbsolutePath.Should().EndWith("/" + FakeId);
                    req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?conflicts=true");
                });
        }

        protected virtual GetDocumentRequest CreateRequest(string id = FakeId, string rev = null)
        {
            return new GetDocumentRequest(id, rev);
        }

        protected virtual void WithHttpRequestFor(GetDocumentRequest request, Action<HttpRequest> a)
        {
            var req = SUT.Create(request);
            a(req);
        }
    }
}