using System.Net.Http;
using FluentAssertions;
using MyCouch;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class PurgeDocumentHttpRequestFactoryTests : UnitTestsOf<PurgeDocumentHttpRequestFactory>
    {
        public PurgeDocumentHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new PurgeDocumentHttpRequestFactory(boostrapper.DocumentSerializerFn());
        }

        [Fact]
        public void It_generates_a_relative_url()
        {
            var r = SUT.Create(new PurgeDocumentRequest("my_doc_id", new[] { "my_doc_rev" }));

            r.RelativeUrl.Should().Be("/_purge");
        }

        [Fact]
        public void It_generates_a_POST()
        {
            var r = SUT.Create(new PurgeDocumentRequest("my_doc_id", new[] { "my_doc_rev" }));

            r.Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void When_id_and_rev_is_specified_It_generates_request_body_for_request_with_id_and_rev()
        {
            var r = SUT.Create(new PurgeDocumentRequest("my_doc_id", new[] { "my_doc_rev" }));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"my_doc_id\":[\"my_doc_rev\"]}");
        }
    }
}