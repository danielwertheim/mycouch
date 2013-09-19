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
        public void When_Keys_are_specified_They_are_included_in_in_the_key_values()
        {
            SUT.Keys = new object[] {
                "fake_key",
                1,
                3.14,
                true,
                false,
                new DateTime(2008, 07, 17, 09, 21, 30, 50)
            };

            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Keys].Should().Be("[\"fake_key\",1,3.14,true,false,\"2008-07-17 09:21:30\"]");
        }

        [Fact]
        public void When_Keys_are_null_It_returns_json_with_no_keys_member_for_GetKeysAsJson()
        {
            SUT.Keys = null;

            SUT.GetKeysAsJsonObject().Should().Be("{}");
        }

        [Fact]
        public void When_Keys_are_empty_It_returns_json_with_no_keys_member_for_GetKeysAsJson()
        {
            SUT.Keys = new string[0];

            SUT.GetKeysAsJsonObject().Should().Be("{}");
        }

        [Fact]
        public void When_Keys_are_specified_It_returns_json_with_the_keys_for_GetKeysAsJson()
        {
            SUT.Keys = new object[] {
                "fake_key",
                1,
                3.14,
                true,
                false,
                new DateTime(2008, 07, 17, 09, 21, 30, 50)
            };

            SUT.GetKeysAsJsonObject().Should().Be("{\"keys\":[\"fake_key\",1,3.14,true,false,\"2008-07-17 09:21:30\"]}");
        }

        [Fact]
        public void When_IncludeDocs_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.IncludeDocs = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.IncludeDocs].Should().Be("true");

            SUT.IncludeDocs = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.IncludeDocs].Should().Be("false");
        }

        [Fact]
        public void When_Descending_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Descending = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Descending].Should().Be("true");

            SUT.Descending = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Descending].Should().Be("false");
        }

        [Fact]
        public void When_InclusiveEnd_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.InclusiveEnd = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.InclusiveEnd].Should().Be("true");

            SUT.InclusiveEnd = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.InclusiveEnd].Should().Be("false");
        }

        [Fact]
        public void When_Reduce_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Reduce = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Reduce].Should().Be("true");

            SUT.Reduce = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Reduce].Should().Be("false");
        }

        [Fact]
        public void When_UpdateSeq_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.UpdateSeq = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.UpdateSeq].Should().Be("true");

            SUT.UpdateSeq = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.UpdateSeq].Should().Be("false");
        }

        [Fact]
        public void When_Group_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Group = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Group].Should().Be("true");

            SUT.Group = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Group].Should().Be("false");
        }

        [Fact]
        public void When_Key_of_string_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Key = "Key1";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("\"Key1\"");

            SUT.Key = "Key2";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("\"Key2\"");
        }

        [Fact]
        public void When_Key_of_int_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Key = 1;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("1");

            SUT.Key = 2;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("2");
        }

        [Fact]
        public void When_Key_of_double_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Key = 3.14;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("3.14");

            SUT.Key = 1.33;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("1.33");
        }

        [Fact]
        public void When_Key_of_bool_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Key = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("true");

            SUT.Key = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("false");
        }

        [Fact]
        public void When_Key_of_datetime_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Key = new DateTime(2008, 07, 17, 09, 21, 30, 50);
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("\"2008-07-17 09:21:30\"");

            SUT.Key = new DateTime(2011, 06, 02, 22, 41, 40, 45);
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Key].Should().Be("\"2011-06-02 22:41:40\"");
        }

        [Fact]
        public void When_StartKey_of_string_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKey = "Key1";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"Key1\"");

            SUT.StartKey = "Key2";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"Key2\"");
        }

        [Fact]
        public void When_StartKey_of_int_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKey = 1;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("1");

            SUT.StartKey = 2;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("2");
        }

        [Fact]
        public void When_StartKey_of_double_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKey = 3.14;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("3.14");

            SUT.StartKey = 1.33;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("1.33");
        }

        [Fact]
        public void When_StartKey_of_bool_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKey = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("true");

            SUT.StartKey = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("false");
        }

        [Fact]
        public void When_StartKey_of_datetime_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKey = new DateTime(2008, 07, 17, 09, 21, 30, 50);
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"2008-07-17 09:21:30\"");

            SUT.StartKey = new DateTime(2011, 06, 02, 22, 41, 40, 45);
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKey].Should().Be("\"2011-06-02 22:41:40\"");
        }

        [Fact]
        public void When_StartKeyDocId_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.StartKeyDocId = "My start key doc id 1";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKeyDocId].Should().Be("\"My start key doc id 1\"");

            SUT.StartKeyDocId = "My start key doc id 2";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.StartKeyDocId].Should().Be("\"My start key doc id 2\"");
        }

        [Fact]
        public void When_EndKey_of_string_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKey = "Key1";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"Key1\"");

            SUT.EndKey = "Key2";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"Key2\"");
        }

        [Fact]
        public void When_EndKey_of_int_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKey = 1;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("1");

            SUT.EndKey = 2;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("2");
        }

        [Fact]
        public void When_EndKey_of_double_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKey = 3.14;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("3.14");

            SUT.EndKey = 1.33;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("1.33");
        }

        [Fact]
        public void When_EndKey_of_bool_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKey = true;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("true");

            SUT.EndKey = false;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("false");
        }

        [Fact]
        public void When_EndKey_of_datetime_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKey = new DateTime(2008, 07, 17, 09, 21, 30, 50);
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"2008-07-17 09:21:30\"");

            SUT.EndKey = new DateTime(2011, 06, 02, 22, 41, 40, 45);
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKey].Should().Be("\"2011-06-02 22:41:40\"");
        }

        [Fact]
        public void When_EndKeyDocId_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.EndKeyDocId = "My end key doc id 1";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKeyDocId].Should().Be("\"My end key doc id 1\"");

            SUT.EndKeyDocId = "My end key doc id 2";
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.EndKeyDocId].Should().Be("\"My end key doc id 2\"");
        }

        [Fact]
        public void When_Skip_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Skip = 5;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Skip].Should().Be("5");

            SUT.Skip = 17;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Skip].Should().Be("17");
        }

        [Fact]
        public void When_Limit_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.Limit = 5;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Limit].Should().Be("5");

            SUT.Limit = 17;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.Limit].Should().Be("17");
        }

        [Fact]
        public void When_GroupLevel_is_assigned_It_gets_included_in_the_key_values()
        {
            SUT.GroupLevel = 1;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.GroupLevel].Should().Be("1");

            SUT.GroupLevel = 3;
            SUT.ToJsonKeyValues()[ViewQueryOptions.KeyValues.GroupLevel].Should().Be("3");
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

            var kvs = SUT.ToJsonKeyValues();

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

        [Fact]
        public void When_not_configured_It_yields_no_key_values()
        {
            var kvs = SUT.ToJsonKeyValues().ToArray();

            kvs.Length.Should().Be(0);
        }
    }
}