using System;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class BulkHttpRequestFactoryTests : UnitTestsOf<BulkHttpRequestFactory>
    {
        public BulkHttpRequestFactoryTests()
        {
            var connection = new DbClientConnectionFake(new Uri("http://foo.com:5984/thedb"), "thedb");
            SUT = new BulkHttpRequestFactory(connection);
        }

        [Fact]
        public void When_the_request_is_empty_It_generates_an_empty_json_array_as_content()
        {
            var r = SUT.Create(new BulkRequest());

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"docs\":[]}");
        }

        [Fact]
        public void When_request_has_deletes_It_generates_a_json_array_with_docs_marked_for_deletion_as_content()
        {
            var r = SUT.Create(new BulkRequest().Delete("1", "1-1").Delete("2", "2-1"));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"docs\":[{\"_id\":\"1\",\"_rev\":\"1-1\",\"_deleted\":true},{\"_id\":\"2\",\"_rev\":\"2-1\",\"_deleted\":true}]}");
        }

        [Fact]
        public void When_request_has_includes_It_generates_a_json_array_with_included_docs_as_content()
        {
            var r = SUT.Create(new BulkRequest().Include(
                "{\"_id\":\"1\",\"name\":\"Test1\"}",
                "{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}"));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"docs\":[{\"_id\":\"1\",\"name\":\"Test1\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}]}");
        }

        [Fact]
        public void When_request_has_includes_and_deletes_It_generates_a_json_array_with_all_docs_as_content()
        {
            var r = SUT.Create(new BulkRequest().Include(
                "{\"_id\":\"1\",\"name\":\"Test1\"}",
                "{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}")
                .Delete("1", "1-1")
                .Delete("2", "2-1"));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"docs\":[{\"_id\":\"1\",\"name\":\"Test1\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"_deleted\":true},{\"_id\":\"2\",\"_rev\":\"2-1\",\"_deleted\":true}]}");
        }
    }
}