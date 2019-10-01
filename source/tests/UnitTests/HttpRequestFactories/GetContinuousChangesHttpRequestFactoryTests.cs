using System;
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
    public class GetContinuousChangesHttpRequestFactoryTests : UnitTestsOf<GetContinuousChangesHttpRequestFactory>
    {
        public GetContinuousChangesHttpRequestFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            var configuration = new SerializationConfiguration(new SerializationContractResolver());
            var serializer = new DefaultSerializer(configuration, new DocumentSerializationMetaProvider(), entityReflector);
            SUT = new GetContinuousChangesHttpRequestFactory(serializer);
        }

        [Fact]
        public void When_Feed_is_assigned_Continuous_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Feed = ChangesFeed.Continuous;

            WithHttpRequestFor(
                request,
                req => req.RelativeUrl.ToTestUriFromRelative().Query.Should().Be("?feed=continuous"));
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