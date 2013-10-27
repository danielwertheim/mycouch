using System;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Requests.Factories;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Cloudant.Requests
{
    public class SearchIndexHttpRequestFactoryTests : UnitTestsOf<SearchIndexHttpRequestFactory>
    {
        public SearchIndexHttpRequestFactoryTests()
        {
            var cnFake = new ConnectionFake(new Uri("https://cdb.foo.com:5984"));

            SUT = new SearchIndexHttpRequestFactory(cnFake);
        }

        [Fact]
        public void When_not_configured_It_yields_no_content_nor_querystring()
        {
            var request = CreateRequest();
            
            WithHttpRequestFor(
                request,
                req => {
                    req.Content.Should().BeNull();
                    req.RequestUri.Query.Should().Be(string.Empty);
                });
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.IncludeDocs = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?include_docs=true"));
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.IncludeDocs = false;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?include_docs=false"));
        }

        [Fact]
        public void When_Descending_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Descending = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?descending=true"));
        }

        [Fact]
        public void When_Descending_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Descending = false;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?descending=false"));
        }

        [Fact]
        public void When_Skip_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Skip = 17;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?skip=17"));
        }

        [Fact]
        public void When_Limit_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Limit = 17;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?limit=17"));
        }

        [Fact]
        public void When_all_options_are_configured_It_yields_a_query_string_accordingly()
        {
            var request = CreateRequest();
            request.Expression = "class:mammal";
            request.IncludeDocs = true;
            request.Descending = true;
            request.Skip = 5;
            request.Limit = 10;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?q=class%3Amammal&include_docs=true&descending=true&limit=10&skip=5"));
        }

        protected virtual SearchIndexRequest CreateRequest()
        {
            return new SearchIndexRequest("foodesigndoc", "indexname");
        }

        protected virtual void WithHttpRequestFor(SearchIndexRequest request, Action<HttpRequestMessage> a)
        {
            using (var req = SUT.Create(request))
                a(req);
        }
    }
}