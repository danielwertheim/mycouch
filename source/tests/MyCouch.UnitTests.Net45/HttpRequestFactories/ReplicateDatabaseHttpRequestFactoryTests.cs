using System;
using System.Collections.Generic;
using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using Xunit;

namespace MyCouch.UnitTests.HttpRequestFactories
{
    public class ReplicateDatabaseHttpRequestFactoryTests : UnitTestsOf<ReplicateDatabaseServerHttpRequestFactory>
    {
        public ReplicateDatabaseHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new ReplicateDatabaseServerHttpRequestFactory(boostrapper.SerializerFn());
        }

        [Fact]
        public void When_source_and_target_is_specified_It_generates_replication_relative_url()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2"));

            r.RelativeUrl.Should().Be("/_replicate");
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

        [Fact]
        public void When_continuous_is_false_It_generates_request_body_with_continuous_false()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Continuous = false });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"continuous\":false}");
        }

        [Fact]
        public void When_filter_is_specified_It_generates_request_body_with_filter()
        {
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { Filter = "mydesigndoc/myfilter" });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"filter\":\"mydesigndoc/myfilter\"}");
        }

        [Fact]
        public void When_query_params_are_specified_It_generates_request_body_with_query_params_object()
        {
            var qp = new Dictionary<string, object> { { "key1", "stringvalue" }, { "key2", 42 }, { "key3", 3.14 }, { "key4", new DateTime(2014, 04, 11, 08, 45, 33) } };
            var r = SUT.Create(new ReplicateDatabaseRequest("fakedb1", "fakedb2") { QueryParams = qp });

            r.Content.ReadAsStringAsync().Result.Should().Be("{\"source\":\"fakedb1\",\"target\":\"fakedb2\",\"query_params\":{"
                + "\"key1\":\"stringvalue\",\"key2\":42,\"key3\":3.14,\"key4\":\"2014-04-11T08:45:33\"}}");
        }
    }
}