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
    public class ShowHttpRequestFactoryTests : UnitTestsOf<ShowHttpRequestFactory>
    {
        public ShowHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new ShowHttpRequestFactory(boostrapper.SerializerFn());
        }

        [Fact]
        public void When_passing_designdoc_name_and_showName_It_should_generate_a_relative_url()
        {
            var r = SUT.Create(new ShowRequest("my_design_doc", "my_show"));

            r.RelativeUrl.Should().Be("/_design/my_design_doc/_show/my_show");
        }

        [Fact]
        public void When_not_configured_It_yields_no_content_nor_querystring()
        {
            var request = CreateRequest();
            
            WithHttpRequestFor(
                request,
                req => {
                    req.Content.Should().BeNull();
                    req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be(string.Empty);
                });
        }

        [Fact]
        public void When_Id_is_assigned_true_It_should_get_included_in_the_relative_url()
        {
            var r = SUT.Create(new ShowRequest("my_design_doc", "my_show").Configure(c => c.Id("myId")));

            r.RelativeUrl.Should().Be("/_design/my_design_doc/_show/my_show/myId");
        }

        [Fact]
        public void When_custom_query_parameter_are_specified_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.CustomQueryParameters = new Dictionary<string, object>
            {
                { "foo", new object[] { "Key1", 42 } }
            };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?foo=%5B%22Key1%22%2C42%5D"));
        }

        protected virtual ShowRequest CreateRequest()
        {
            return new ShowRequest("foodesigndoc", "barviewname");
        }

        protected virtual void WithHttpRequestFor(ShowRequest query, Action<HttpRequest> a)
        {
            var req = SUT.Create(query);
            a(req);
        }
    }
}