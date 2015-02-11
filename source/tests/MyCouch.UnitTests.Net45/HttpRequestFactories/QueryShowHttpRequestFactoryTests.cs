using System;
using System.Collections.Generic;
using FluentAssertions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests.HttpRequestFactories
{
    public class QueryShowHttpRequestFactoryTests : UnitTestsOf<QueryShowHttpRequestFactory>
    {
        public QueryShowHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new QueryShowHttpRequestFactory(boostrapper.DocumentSerializerFn());
        }

        [Fact]
        public void When_passing_ddoc_name_and_show_name_It_should_generate_a_relative_url()
        {
            var r = SUT.Create(new QueryShowRequest("my_design_doc", "my_show"));

            r.RelativeUrl.Should().Be("/_design/my_design_doc/_show/my_show");
        }

        [Fact]
        public void When_not_configured_It_yields_no_content_nor_querystring()
        {
            var request = CreateRequest();

            WithHttpRequestFor(
                request,
                req =>
                {
                    req.Content.Should().BeNull();
                    req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be(string.Empty);
                });
        }

        [Fact]
        public void When_docId_is_specified_Then_It_should_generate_a_relative_url()
        {
            var request = new QueryShowRequest("my_design_doc", "my_show");
            request.DocId = "my_doc_id";
            var r = SUT.Create(request);

            r.RelativeUrl.Should().Be("/_design/my_design_doc/_show/my_show/my_doc_id");
        }

        [Fact]
        public void When_custom_query_parameters_are_specified_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.CustomQueryParameters = new Dictionary<string, object>
            {
                { "myint", 42 },
                { "mystring", "test"},
                { "mybool", true},
                { "mydatetime", new DateTime(2014,1,1,13,14,15,16) }
            };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?myint=42&mystring=test&mybool=True&mydatetime=2014-01-01T13%3A14%3A15"));
        }

        protected virtual QueryShowRequest CreateRequest()
        {
            return new QueryShowRequest("foodesigndoc", "barshowname");
        }

        protected virtual void WithHttpRequestFor(QueryShowRequest query, Action<HttpRequest> a)
        {
            var req = SUT.Create(query);
            a(req);
        }
    }
}