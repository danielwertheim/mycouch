using FluentAssertions;
using MyCouch.Querying;
using Xunit;
using System.Linq;

namespace MyCouch.UnitTests.Querying
{
    public class ViewQueryOptionsTests : UnitTestsOf<ViewQueryOptions>
    {
        public ViewQueryOptionsTests()
        {
            SUT = new ViewQueryOptions();
        }

        [Fact]
        public void When_not_configured_It_yields_no_key_values()
        {
            var kvs = SUT.ToKeyValues().ToArray();

            kvs.Length.Should().Be(0);
        }

        [Fact]
        public void When_keys_are_null_It_returns_false_for_HasKeys()
        {
            SUT.Keys = null;

            SUT.HasKeys.Should().BeFalse();
        }

        [Fact]
        public void When_keys_are_empty_It_returns_false_for_HasKeys()
        {
            SUT.Keys = new string[0];

            SUT.HasKeys.Should().BeFalse();
        }

        [Fact]
        public void When_keys_are_specified_It_returns_true_for_HasKeys()
        {
            SUT.Keys = new[] { "fake_key" };

            SUT.HasKeys.Should().BeTrue();
        }

        [Fact]
        public void When_keys_are_null_It_returns_json_with_no_keys_member_for_GetKeysAsJson()
        {
            SUT.Keys = null;

            SUT.GetKeysAsJson().Should().Be("{}");
        }

        [Fact]
        public void When_keys_are_empty_It_returns_json_with_no_keys_member_for_GetKeysAsJson()
        {
            SUT.Keys = new string[0];

            SUT.GetKeysAsJson().Should().Be("{}");
        }

        [Fact]
        public void When_keys_are_specified_It_returns_json_with_the_keys_for_GetKeysAsJson()
        {
            SUT.Keys = new[] { "fake_key1", "fake_key2" };

            SUT.GetKeysAsJson().Should().Be("{\"keys\":[\"fake_key1\",\"fake_key2\"]}");
        }

        [Fact]
        public void When_configured_It_yields_correct_key_values()
        {
            SUT.Stale = Stale.UpdateAfter;
            SUT.IncludeDocs = true;
            SUT.Descending = true;
            SUT.Key = "Key1";
            SUT.Keys = new[] { "Key1", "Key2" };
            SUT.StartKey = "My start key";
            SUT.StartKeyDocId = "My start key doc id";
            SUT.EndKey = "My end key";
            SUT.EndKeyDocId = "My end key doc id";
            SUT.InclusiveEnd = false;
            SUT.Skip = 5;
            SUT.Limit = 10;
            SUT.Reduce = false;
            SUT.UpdateSeq = true;
            SUT.Group = true;
            SUT.GroupLevel = 3;

            var kvs = SUT.ToKeyValues().ToDictionary(i => i.Key, i => i.Value);

            kvs.Count.Should().Be(16);
            kvs["stale"].Should().Be("\"update_after\"");
            kvs["include_docs"].Should().Be("true");
            kvs["descending"].Should().Be("true");
            kvs["key"].Should().Be("\"Key1\"");
            kvs["keys"].Should().Be("[\"Key1\",\"Key2\"]");
            kvs["startkey"].Should().Be("\"My start key\"");
            kvs["startkey_docid"].Should().Be("\"My start key doc id\"");
            kvs["endkey"].Should().Be("\"My end key\"");
            kvs["endkey_docid"].Should().Be("\"My end key doc id\"");
            kvs["inclusive_end"].Should().Be("false");
            kvs["skip"].Should().Be("5");
            kvs["limit"].Should().Be("10");
            kvs["reduce"].Should().Be("false");
            kvs["update_seq"].Should().Be("true");
            kvs["group"].Should().Be("true");
            kvs["group_level"].Should().Be("3");
        }
    }
}