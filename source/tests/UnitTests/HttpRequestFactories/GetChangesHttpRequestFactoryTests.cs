﻿using System;
using FluentAssertions;
using MyCouch;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;
using MyCouch.Testing;
using Xunit;

namespace UnitTests.HttpRequestFactories
{
    public class GetChangesHttpRequestFactoryTests : UnitTestsOf<GetChangesHttpRequestFactory>
    {
        public GetChangesHttpRequestFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            var configuration = new SerializationConfiguration(new SerializationContractResolver());
            var serializer = new DefaultSerializer(configuration, new DocumentSerializationMetaProvider(), entityReflector);
            SUT = new GetChangesHttpRequestFactory(serializer);
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
        public void When_Feed_is_assigned_Normal_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Feed = ChangesFeed.Normal;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?feed=normal"));
        }

        [Fact]
        public void When_Feed_is_assigned_Longpoll_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Feed = ChangesFeed.Longpoll;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?feed=longpoll"));
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.IncludeDocs = true;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?include_docs=true"));
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.IncludeDocs = false;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?include_docs=false"));
        }

        [Fact]
        public void When_Descending_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Descending = true;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?descending=true"));
        }

        [Fact]
        public void When_Descending_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Descending = false;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?descending=false"));
        }

        [Fact]
        public void When_Since_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Since = "17";

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?since=17"));
        }

        [Fact]
        public void When_Limit_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Limit = 17;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?limit=17"));
        }

        [Fact]
        public void When_HeartBeat_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Heartbeat = 17;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?heartbeat=17"));
        }

        [Fact]
        public void When_Timeout_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Timeout = 17;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?timeout=17"));
        }

        [Fact]
        public void When_Filter_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Filter = "app/important";

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?filter=app%2Fimportant"));
        }

        [Fact]
        public void When_DocIds_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.DocIds = new[] { "7bed7f2dc89b45479b9ac09504fff1eb" };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be(
                    "?doc_ids=%5B%227bed7f2dc89b45479b9ac09504fff1eb%22%5D&filter=_doc_ids"));
        }

        [Fact]
        public void When_Style_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Style = ChangesStyle.AllDocs;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?style=all_docs"));
        }


        [Fact]
        public void When_Custom_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Custom = new System.Collections.Generic.Dictionary<string, string> { 
                { "par1", "val1" }
            };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?par1=val1"));
        }

        [Fact]
        public void When_2_Custom_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Custom = new System.Collections.Generic.Dictionary<string, string> {
                { "par1", "val1" },
                { "par2", "val2" }
            };

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?par1=val1&par2=val2"));
        }

        protected virtual GetChangesRequest CreateRequest()
        {
            return new GetChangesRequest();
        }

        protected virtual void WithHttpRequestFor(GetChangesRequest request, Action<HttpRequest> a)
        {
            var req = SUT.Create(request);
            a(req);
        }
    }
}