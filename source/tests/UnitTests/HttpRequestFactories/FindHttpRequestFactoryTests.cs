namespace UnitTests.HttpRequestFactories
{
    using System;
using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MyCouch;
    using MyCouch.HttpRequestFactories;
    using MyCouch.Requests;
    using MyCouch.Testing;
    using Xunit;
using static MyCouch.Testing.TestData.ClientTestData.Attachments;

    public sealed class FindHttpRequestFactoryTests : UnitTestsOf<FindHttpRequestFactory>
    {
        private const string SelectorExpression = "{\"Type\": \"Unknown\"}";
        private const string Bookmark = "g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg";

        public FindHttpRequestFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new FindHttpRequestFactory(boostrapper.SerializerFn());
        }

        [Fact]
        public void When_SelectorExpression_is_not_assigned_It_should_thrown_an_ArgumentNullException()
        {
            var request = CreateRequest();
            request.SelectorExpression = null;

            Action action = () => SUT.Create(request);

            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public async Task When_Limit_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Limit = int.MaxValue;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"limit\": 2147483647}");
        }

        [Fact]
        public async Task When_Skip_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Skip = int.MaxValue;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"skip\": 2147483647}");
        }

        [Fact]
        public async Task When_UseIndex_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.UseIndex = "indexToUse";

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"use_index\": \"indexToUse\"}");
        }

        [Fact]
        public async Task When_Stable_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Stable = true;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"stable\": true}");
        }

        [Fact]
        public async Task When_Update_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Update = true;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"update\": true}");
        }

        [Fact]
        public async Task When_Bookmark_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Bookmark = Bookmark;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be($"{{\"selector\": {{\"Type\": \"Unknown\"}},\"bookmark\": \"{Bookmark}\"}}");
        }

        [Fact]
        public async Task When_Conflicts_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Conflicts = true;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"conflicts\": true}");
        }

        [Fact]
        public async Task When_ReadQuorum_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.ReadQuorum = 2;

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"r\": 2}");
        }

        [Fact]
        public async Task When_Fields_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Fields = new []{ "one", "two" };

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"fields\": [\"one\",\"two\"]}");
        }

        [Fact]
        public async Task When_Sort_is_assigned_It_should_included_in_the_request_body()
        {
            var request = CreateRequest();
            request.Sort = new List<SortableField>
            {
                new SortableField("one", SortDirection.Desc)
            };

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"},\"sort\": [{\"one\":\"desc\"}]}");
        }

        [Fact]
        public async Task When_only_SelectorExpression_is_assigned_It_should_include_only_it_in_the_request_body()
        {
            var request = CreateRequest();

            var body = await SUT.Create(request).Content.ReadAsStringAsync();

            body.Should().Be("{\"selector\": {\"Type\": \"Unknown\"}}");
        }
        
        private FindRequest CreateRequest()
        {
            return new FindRequest
            {
                SelectorExpression = SelectorExpression
            };
        }
    }
}
