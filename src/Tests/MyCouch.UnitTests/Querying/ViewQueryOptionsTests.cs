using System;
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
        public void When_Keys_are_null_It_returns_false_for_HasKeys()
        {
            SUT.Keys = null;

            SUT.HasKeys.Should().BeFalse();
        }

        [Fact]
        public void When_Keys_are_empty_It_returns_false_for_HasKeys()
        {
            SUT.Keys = new string[0];

            SUT.HasKeys.Should().BeFalse();
        }

        [Fact]
        public void When_Keys_are_specified_It_returns_true_for_HasKeys()
        {
            SUT.Keys = new[] { "fake_key" };

            SUT.HasKeys.Should().BeTrue();
        }

        [Fact]
        public void When_Keys_are_null_It_returns_json_with_no_keys_member_for_GetKeysAsJson()
        {
            SUT.Keys = null;

            SUT.GetKeysAsJson().Should().Be("{}");
        }

        [Fact]
        public void When_Keys_are_empty_It_returns_json_with_no_keys_member_for_GetKeysAsJson()
        {
            SUT.Keys = new string[0];

            SUT.GetKeysAsJson().Should().Be("{}");
        }

        [Fact]
        public void When_Keys_are_specified_It_returns_json_with_the_keys_for_GetKeysAsJson()
        {
            SUT.Keys = new[] { "fake_key1", "fake_key2" };

            SUT.GetKeysAsJson().Should().Be("{\"keys\":[\"fake_key1\",\"fake_key2\"]}");
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.IncludeDocs = true;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.IncludeDocs].Should().Be("true");

            SUT.IncludeDocs = false;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.IncludeDocs].Should().Be("false");
        }

        [Fact]
        public void When_Descending_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Descending = true;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Descending].Should().Be("true");

            SUT.Descending = false;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Descending].Should().Be("false");
        }

        [Fact]
        public void When_InclusiveEnd_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.InclusiveEnd = true;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.InclusiveEnd].Should().Be("true");

            SUT.InclusiveEnd = false;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.InclusiveEnd].Should().Be("false");
        }

        [Fact]
        public void When_Reduce_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Reduce = true;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Reduce].Should().Be("true");

            SUT.Reduce = false;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Reduce].Should().Be("false");
        }

        [Fact]
        public void When_UpdateSeq_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.UpdateSeq = true;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.UpdateSeq].Should().Be("true");

            SUT.UpdateSeq = false;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.UpdateSeq].Should().Be("false");
        }

        [Fact]
        public void When_Group_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Group = true;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Group].Should().Be("true");

            SUT.Group = false;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Group].Should().Be("false");
        }

        [Fact]
        public void When_Key_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Key = "Key1";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("\"Key1\"");

            SUT.Key = "Key2";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("\"Key2\"");
        }

        [Fact]
        public void When_Keys_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Keys = new[] { "Key1", "Key2" };
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Keys].Should().Be("[\"Key1\",\"Key2\"]");

            SUT.Keys = new[] { "Key1", "Key2", "Key3" };
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Keys].Should().Be("[\"Key1\",\"Key2\",\"Key3\"]");
        }

        [Fact]
        public void When_StartKey_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKey = "My start key 1";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"My start key 1\"");

            SUT.StartKey = "My start key 2";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"My start key 2\"");
        }

        [Fact]
        public void When_StartKeyDocId_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKeyDocId = "My start key doc id 1";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.StartKeyDocId].Should().Be("\"My start key doc id 1\"");

            SUT.StartKeyDocId = "My start key doc id 2";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.StartKeyDocId].Should().Be("\"My start key doc id 2\"");
        }

        [Fact]
        public void When_EndKey_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKey = "My end key 1";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"My end key 1\"");

            SUT.EndKey = "My end key 2";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"My end key 2\"");
        }

        [Fact]
        public void When_EndKeyDocId_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKeyDocId = "My end key doc id 1";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.EndKeyDocId].Should().Be("\"My end key doc id 1\"");

            SUT.EndKeyDocId = "My end key doc id 2";
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.EndKeyDocId].Should().Be("\"My end key doc id 2\"");
        }

        [Fact]
        public void When_Skip_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Skip = 5;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Skip].Should().Be("5");

            SUT.Skip = 17;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Skip].Should().Be("17");
        }

        [Fact]
        public void When_Limit_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Limit = 5;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Limit].Should().Be("5");

            SUT.Limit = 17;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.Limit].Should().Be("17");
        }

        [Fact]
        public void When_GroupLevel_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.GroupLevel = 1;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.GroupLevel].Should().Be("1");

            SUT.GroupLevel = 3;
            SUT.ToKeyValues()[ViewQueryOptions.KeyValues.GroupLevel].Should().Be("3");
        }

        [Fact]
        public void When_all_options_are_configures_It_yields_a_key_value_dictionary_with_all_options()
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

            var kvs = SUT.ToKeyValues();

            kvs.Count.Should().Be(16);
            kvs[ViewQueryOptions.KeyValues.Stale].Should().Be("\"update_after\"");
            kvs[ViewQueryOptions.KeyValues.IncludeDocs].Should().Be("true");
            kvs[ViewQueryOptions.KeyValues.Descending].Should().Be("true");
            kvs[ViewQueryOptions.KeyValues.Key].Should().Be("\"Key1\"");
            kvs[ViewQueryOptions.KeyValues.Keys].Should().Be("[\"Key1\",\"Key2\"]");
            kvs[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"My start key\"");
            kvs[ViewQueryOptions.KeyValues.StartKeyDocId].Should().Be("\"My start key doc id\"");
            kvs[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"My end key\"");
            kvs[ViewQueryOptions.KeyValues.EndKeyDocId].Should().Be("\"My end key doc id\"");
            kvs[ViewQueryOptions.KeyValues.InclusiveEnd].Should().Be("false");
            kvs[ViewQueryOptions.KeyValues.Skip].Should().Be("5");
            kvs[ViewQueryOptions.KeyValues.Limit].Should().Be("10");
            kvs[ViewQueryOptions.KeyValues.Reduce].Should().Be("false");
            kvs[ViewQueryOptions.KeyValues.UpdateSeq].Should().Be("true");
            kvs[ViewQueryOptions.KeyValues.Group].Should().Be("true");
            kvs[ViewQueryOptions.KeyValues.GroupLevel].Should().Be("3");
        }
    }
}