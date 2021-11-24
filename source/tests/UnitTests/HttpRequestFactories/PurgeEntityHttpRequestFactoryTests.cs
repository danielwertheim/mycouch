using System.Net.Http;
using FluentAssertions;
using MyCouch;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class PurgeEntityHttpRequestFactoryTests : UnitTestsOf<PurgeEntityHttpRequestFactory>
    {
        public PurgeEntityHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new PurgeEntityHttpRequestFactory(boostrapper.EntityReflectorFn(), boostrapper.DocumentSerializerFn());
        }

        [Fact]
        public void It_generates_a_relative_url()
        {
            var model = new Model { Id = "my_doc_id", Rev = "my_doc_rev" };
            var r = SUT.Create(new PurgeEntityRequest<Model>(model));

            r.RelativeUrl.Should().Be("/_purge");
        }

        [Fact]
        public void It_generates_a_POST()
        {
            var model = new Model { Id = "my_doc_id", Rev = "my_doc_rev" };
            var r = SUT.Create(new PurgeEntityRequest<Model>(model));

            r.Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void When_id_and_rev_is_specified_It_generates_request_body_for_request_with_id_and_rev()
        {
            var model = new Model { Id = "my_doc_id", Rev = "my_doc_rev" };
            var r = SUT.Create(new PurgeEntityRequest<Model>(model));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"my_doc_id\":[\"my_doc_rev\"]}");
        }

        private class Model
        {
            public string Id { get; set; }
            public string Rev { get; set; }
        }
    }
}