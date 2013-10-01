using System;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Builders;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Requests
{
    public class QueryViewHttpRequestBuilderTests : UnitTestsOf<QueryViewHttpRequestBuilder>
    {
        public QueryViewHttpRequestBuilderTests()
        {
            var cnFake = new ConnectionFake(new Uri("https://cdb.foo.com:5984"));

            SUT = new QueryViewHttpRequestBuilder(cnFake);
        }

        [Fact]
        public void When_not_configured_It_yields_no_content_nor_querystring()
        {
            var request = CreateRequest();
            
            WithHttpRequestFor(
                request,
                req => {
                    req.Content.Should().BeNull();
                    req.RequestUri.Query.Should().Be("?");
                });
        }

        [Fact]
        public void When_Keys_are_specified_Then_they_should_be_included_in_the_content_as_json_object()
        {
            var request = CreateRequest();
            request.Keys = new object[] {
                "fake_key",
                1,
                3.14,
                true,
                false,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                new object[] {"complex1", 42}
            };

            WithHttpRequestFor(
                request,
                req => req.Content.ReadAsStringAsync().Result.Should().Be("{\"keys\":[\"fake_key\",1,3.14,true,false,\"2008-07-17T09:21:30\",[\"complex1\",42]]}"));
        }

        [Fact]
        public void When_Keys_are_null_Then_the_content_should_become_null()
        {
            var request = CreateRequest();
            request.Keys = null;

            WithHttpRequestFor(
                request,
                req => req.Content.Should().BeNull());
        }

        [Fact]
        public void When_Keys_are_empty_Then_the_content_should_become_null()
        {
            var request = CreateRequest();
            request.Keys = new object[0];

            WithHttpRequestFor(
                request,
                req => req.Content.Should().BeNull());
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
        public void When_InclusiveEnd_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.InclusiveEnd = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?inclusive_end=true"));
        }

        [Fact]
        public void When_InclusiveEnd_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.InclusiveEnd = false;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?inclusive_end=false"));
        }

        [Fact]
        public void When_Reduce_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Reduce = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?reduce=true"));
        }

        [Fact]
        public void When_Reduce_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Reduce = false;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?reduce=false"));
        }

        [Fact]
        public void When_UpdateSeq_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.UpdateSeq = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?update_seq=true"));
        }

        [Fact]
        public void When_UpdateSeq_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.UpdateSeq = false;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?update_seq=false"));
        }

        [Fact]
        public void When_Group_is_assigned_true_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Group = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?group=true"));
        }

        [Fact]
        public void When_Group_is_assigned_false_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Group = false;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?group=false"));
        }

        [Fact]
        public void When_complex_Key_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Key = new object[] { "Key1", 42 };

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?key=%5B%22Key1%22%2C42%5D"));
        }

        [Fact]
        public void When_Key_of_string_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Key = "Key1";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?key=%22Key1%22"));
        }

        [Fact]
        public void When_Key_of_int_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Key = 42;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?key=42"));
        }

        [Fact]
        public void When_Key_of_double_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Key = 3.14;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?key=3.14"));
        }

        [Fact]
        public void When_Key_of_boolean_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Key = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?key=true"));
        }

        [Fact]
        public void When_Key_of_datetime_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.Key = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?key=%222008-07-17T09%3A21%3A30%22"));
        }

        [Fact]
        public void When_complex_StartKey_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKey = new object[] { "Key1", 42 };

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey=%5B%22Key1%22%2C42%5D"));
        }

        [Fact]
        public void When_StartKey_of_string_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKey = "Key1";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey=%22Key1%22"));
        }

        [Fact]
        public void When_StartKey_of_int_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKey = 42;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey=42"));
        }

        [Fact]
        public void When_StartKey_of_double_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKey = 3.14;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey=3.14"));
        }

        [Fact]
        public void When_StartKey_of_boolean_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKey = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey=true"));
        }

        [Fact]
        public void When_StartKey_of_datetime_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKey = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey=%222008-07-17T09%3A21%3A30%22"));
        }

        [Fact]
        public void When_StartKeyDocId_of_datetime_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.StartKeyDocId = "My start key doc id 1";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?startkey_docid=%22My%20start%20key%20doc%20id%201%22"));
        }

        [Fact]
        public void When_complex_EndKey_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKey = new object[] { "Key1", 42 };

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey=%5B%22Key1%22%2C42%5D"));
        }

        [Fact]
        public void When_EndKey_of_string_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKey = "Key1";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey=%22Key1%22"));
        }

        [Fact]
        public void When_EndKey_of_int_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKey = 42;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey=42"));
        }

        [Fact]
        public void When_EndKey_of_double_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKey = 3.14;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey=3.14"));
        }

        [Fact]
        public void When_EndKey_of_boolean_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKey = true;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey=true"));
        }

        [Fact]
        public void When_EndKey_of_datetime_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKey = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey=%222008-07-17T09%3A21%3A30%22"));
        }

        [Fact]
        public void When_EndKeyDocId_of_datetime_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.EndKeyDocId = "My end key doc id 1";

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?endkey_docid=%22My%20end%20key%20doc%20id%201%22"));
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
        public void When_GroupLevel_is_assigned_It_should_get_included_in_the_querystring()
        {
            var request = CreateRequest();
            request.GroupLevel = 3;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?group_level=3"));
        }

        [Fact]
        public void When_all_options_except_Keys_are_configured_It_yields_a_query_string_accordingly()
        {
            var request = CreateRequest();
            request.Stale = Stale.UpdateAfter;
            request.IncludeDocs = true;
            request.Descending = true;
            request.Key = "Key1";
            request.StartKey = "My start key";
            request.StartKeyDocId = "My start key doc id";
            request.EndKey = "My end key";
            request.EndKeyDocId = "My end key doc id";
            request.InclusiveEnd = true;
            request.Skip = 5;
            request.Limit = 10;
            request.Reduce = true;
            request.UpdateSeq = true;
            request.Group = true;
            request.GroupLevel = 3;

            WithHttpRequestFor(
                request,
                req => req.RequestUri.Query.Should().Be("?include_docs=true&descending=true&reduce=true&inclusive_end=true&update_seq=true&group=true&group_level=3&stale=%22update_after%22&key=%22Key1%22&startkey=%22My%20start%20key%22&startkey_docid=%22My%20start%20key%20doc%20id%22&endkey=%22My%20end%20key%22&endkey_docid=%22My%20end%20key%20doc%20id%22&limit=10&skip=5"));
        }

        protected virtual QueryViewRequest CreateRequest()
        {
            return new QueryViewRequest("foodesigndoc", "barviewname");
        }

        protected virtual void WithHttpRequestFor(QueryViewRequest query, Action<HttpRequestMessage> a)
        {
            using (var req = SUT.Create(query))
                a(req);
        }
    }
}