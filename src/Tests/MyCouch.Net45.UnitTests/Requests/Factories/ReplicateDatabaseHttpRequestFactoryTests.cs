using System;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests.Factories
{
    public class ReplicateDatabaseHttpRequestFactoryTests : UnitTestsOf<ReplicateDatabaseHttpRequestFactory>
    {
        public ReplicateDatabaseHttpRequestFactoryTests()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);
        }

        [Fact]
        public void When_source_and_target_is_specified_It_generates_replication_url()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/_replicate");
        }

        [Fact]
        public void When_source_and_target_is_specified_It_generates_request_body_for_request_with_source_and_target()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2"));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\"}");
        }

        [Fact]
        public void When_create_target_is_true_It_generates_request_body_with_create_target_true()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { CreateTarget = true });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"create_target\":true}");
        }

        [Fact]
        public void When_create_target_is_false_It_generates_request_body_with_create_target_false()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { CreateTarget = false });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"create_target\":false}");
        }

        [Fact]
        public void When_cancel_is_true_It_generates_request_body_with_cancel_true()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Cancel = true });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"cancel\":true}");
        }

        [Fact]
        public void When_cancel_is_false_It_generates_request_body_with_cancel_false()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Cancel = false });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"cancel\":false}");
        }

        [Fact]
        public void When_proxy_is_specified_It_generates_request_body_with_proxy()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Proxy = "https://myproxy.com" });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"proxy\":\"https://myproxy.com\"}");
        }

        [Fact]
        public void When_doc_ids_are_specified_It_generates_request_body_with_doc_ids()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { DocIds = new[] { "d1", "d2" } });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"doc_ids\":[\"d1\",\"d2\"]}");
        }

        [Fact]
        public void When_continuous_is_true_It_generates_request_body_with_continuous_true()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Continuous = true });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"continuous\":true}");
        }
    }
}