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
        [Fact]
        public void When_used_with_ServerClientConnection_It_generates_replication_url()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2"));

            r.RequestUri.AbsoluteUri.Should().Be("http://foo.com:5984/_replicate");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_It_generates_request_body_for_request_with_source_and_target()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2"));

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\"}");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_and_CreateTarget_is_true_It_generates_request_body_with_create_target_true()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { CreateTarget = true });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"create_target\":true}");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_and_CreateTarget_is_false_It_generates_request_body_with_create_target_false()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { CreateTarget = false });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"create_target\":false}");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_and_Cancel_is_true_It_generates_request_body_with_cancel_true()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Cancel = true });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"cancel\":true}");
        }

        [Fact]
        public void When_used_with_ServerClientConnection_and_Cancel_is_false_It_generates_request_body_with_cancel_false()
        {
            var connection = new ServerClientConnectionFake(new Uri("http://foo.com:5984"));
            SUT = new ReplicateDatabaseHttpRequestFactory(connection);

            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Cancel = false });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"cancel\":false}");
        }
    }
}