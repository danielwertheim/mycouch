using System;
using FluentAssertions;
using MyCouch.Cloudant;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Net;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests.Cloudant.HttpRequestFactories
{
    public class SearchIndexHttpRequestFactoryTests : UnitTestsOf<SearchIndexHttpRequestFactory>
    {
        public SearchIndexHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchCloudantClientBootstrapper();
            SUT = new SearchIndexHttpRequestFactory(boostrapper.DocumentSerializerFn(), boostrapper.SerializerFn());
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
                    req.RelativeUrl.ToUnescapedQuery().Should().Be(string.Empty);
                });
        }

        [Fact]
        public void When_Expression_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Expression = "Some value";

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?q=Some value"));
        }

        [Fact]
        public void When_Bookmark_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Bookmark = "g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg";

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?bookmark=g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg"));
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.IncludeDocs = true;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?include_docs=true"));
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.IncludeDocs = false;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?include_docs=false"));
        }

        [Fact]
        public void When_Stale_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Stale = Stale.UpdateAfter;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?stale=update_after"));
        }

        [Fact]
        public void When_Limit_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Limit = 17;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?limit=17"));
        }

        [Fact]
        public void When_Sort_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Sort = new[] { "a", "b" };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?sort=[\"a\",\"b\"]"));
        }

        [Fact]
        public void When_GroupSort_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.GroupSort = new[] { "a", "b" };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?group_sort=[\"a\",\"b\"]"));
        }

        [Fact]
        public void When_GroupLimit_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.GroupLimit = 17;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?group_limit=17"));
        }

        [Fact]
        public void When_GroupField_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.GroupField = "Some value";

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?group_field=Some value"));
        }

        [Fact]
        public void When_more_than_one_option_is_configured_It_yields_a_query_string_accordingly()
        {
            var request = CreateRequest();
            request.Expression = "class:mammal";
            request.IncludeDocs = true;
            request.Limit = 10;
            request.Sort = new[] { "diet<string>", "-min_length<number>" };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?q=class:mammal&sort=[\"diet<string>\",\"-min_length<number>\"]&limit=10&include_docs=true"));
        }

        [Fact]
        public void When_Counts_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Counts = new[] { "a", "b" };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToUnescapedQuery().Should().Be("?counts=[\"a\",\"b\"]"));
        }

        protected virtual SearchIndexRequest CreateRequest()
        {
            return new SearchIndexRequest("foodesigndoc", "indexname");
        }

        protected virtual void WithHttpRequestFor(SearchIndexRequest request, Action<HttpRequest> a)
        {
            var req = SUT.Create(request);
            a(req);
        }
    }
}