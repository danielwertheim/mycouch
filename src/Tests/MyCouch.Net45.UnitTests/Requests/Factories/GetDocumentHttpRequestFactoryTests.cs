using System;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class GetDocumentHttpRequestFactoryTests : UnitTestsOf<GetDocumentHttpRequestFactory>
    {
        protected const string FakeId = "someId";
        
        public GetDocumentHttpRequestFactoryTests()
        {
            var cnFake = new DbClientConnectionFake(new Uri("https://cdb.foo.com:5984/mydb"), "mydb");

            SUT = new GetDocumentHttpRequestFactory(cnFake);
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
                    req.RequestUri.AbsolutePath.Should().EndWith("/" + FakeId);
                    req.RequestUri.Query.Should().Be("?rev=" + fakeRev + "&conflicts=true");
                });
        }

        [Fact]
        public void When_configured_with_id_It_yields_url_with_id()
        {
            var request = CreateRequest();

            WithHttpRequestFor(
                request,
                req => req.RequestUri.AbsolutePath.Should().EndWith("/" + FakeId));
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
                    req.RequestUri.AbsolutePath.Should().EndWith("/" + FakeId);
                    req.RequestUri.Query.Should().Be("?rev=" + fakeRev);
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
                    req.RequestUri.AbsolutePath.Should().EndWith("/" + FakeId);
                    req.RequestUri.Query.Should().Be("?conflicts=true");
                });
        }

        protected virtual GetDocumentRequest CreateRequest(string id = FakeId, string rev = null)
        {
            return new GetDocumentRequest(id, rev);
        }

        protected virtual void WithHttpRequestFor(GetDocumentRequest request, Action<HttpRequestMessage> a)
        {
            using (var req = SUT.Create(request))
                a(req);
        }
    }
}