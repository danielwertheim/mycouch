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
                req =>
                {
                    req.Content.Should().BeNull();
                    req.RequestUri.Query.Should().Be(string.Empty);
                });
        }

        [Fact]
        public void When_Expression_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Expression = "Some value";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?q=Some%20value"));
        }

        [Fact]
        public void When_Bookmark_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Bookmark = "g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?bookmark=g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg"));
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
        public void When_Stale_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Stale = Stale.UpdateAfter;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?stale=update_after"));
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
        public void When_Sort_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Sort.AddRange(new[] { "diet<string>", "-min_length<number>" });

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?sort=%5B%22diet%3Cstring%3E%22%2C%22-min_length%3Cnumber%3E%22%5D"));
        }

        [Fact]
        public void When_all_options_are_configured_It_yields_a_query_string_accordingly()
        {
            var request = CreateRequest();
            request.Expression = "class:mammal";
            request.Bookmark = "Some bookmark";
            request.Stale = Stale.UpdateAfter;
            request.IncludeDocs = true;
            request.Limit = 10;
            request.Sort.AddRange(new[] { "diet<string>", "-min_length<number>" });

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?q=class%3Amammal&sort=%5B%22diet%3Cstring%3E%22%2C%22-min_length%3Cnumber%3E%22%5D&bookmark=Some%20bookmark&stale=update_after&limit=10&include_docs=true"));
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