using System;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Configurators;
using Xunit;

namespace MyCouch.UnitTests.Requests
{
    public class QueryViewRequestConfiguratorTests : UnitTestsOf<QueryViewRequestConfigurator>
    {
        private readonly QueryViewRequest _request;

        public QueryViewRequestConfiguratorTests()
        {
            _request = new QueryViewRequest("foodesigndocument", "barviewname");

            SUT = new QueryViewRequestConfigurator(_request);
        }

        [Fact]
        public void When_config_of_Descending_It_configures_underlying_options_Descending()
        {
            const bool configuredValue = true;

            SUT.Descending(configuredValue);

            _request.Descending.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_IncludeDocs_It_configures_underlying_options_IncludeDocs()
        {
            const bool configuredValue = true;

            SUT.IncludeDocs(configuredValue);

            _request.IncludeDocs.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Skip_It_configures_underlying_options_Skip()
        {
            const int configuredValue = 10;

            SUT.Skip(configuredValue);

            _request.Skip.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Limit_It_configures_underlying_options_Limit()
        {
            const int configuredValue = 10;

            SUT.Limit(configuredValue);

            _request.Limit.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_string_It_configures_underlying_options_Key()
        {
            const string configuredValue = "Fake key 1";

            SUT.Key(configuredValue);

            _request.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_int_It_configures_underlying_options_Key()
        {
            const int configuredValue = 42;

            SUT.Key(configuredValue);

            _request.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_double_It_configures_underlying_options_Key()
        {
            const double configuredValue = 3.14;

            SUT.Key(configuredValue);

            _request.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            SUT.Key(configuredValue);

            _request.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_boolean_It_configures_underlying_options_Key()
        {
            const bool configuredValue = true;

            SUT.Key(configuredValue);

            _request.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_complex_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true
            };

            SUT.Key(configuredValue);

            _request.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_string_It_configures_underlying_options_Key()
        {
            const string configuredValue = "Fake key 1";

            SUT.StartKey(configuredValue);

            _request.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_int_It_configures_underlying_options_Key()
        {
            const int configuredValue = 42;

            SUT.StartKey(configuredValue);

            _request.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_double_It_configures_underlying_options_Key()
        {
            const double configuredValue = 3.14;

            SUT.StartKey(configuredValue);

            _request.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            SUT.StartKey(configuredValue);

            _request.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_boolean_It_configures_underlying_options_Key()
        {
            const bool configuredValue = true;

            SUT.StartKey(configuredValue);

            _request.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_complex_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true
            };

            SUT.StartKey(configuredValue);

            _request.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_string_It_configures_underlying_options_Key()
        {
            const string configuredValue = "Fake key 1";

            SUT.EndKey(configuredValue);

            _request.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_int_It_configures_underlying_options_Key()
        {
            const int configuredValue = 42;

            SUT.EndKey(configuredValue);

            _request.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_double_It_configures_underlying_options_Key()
        {
            const double configuredValue = 3.14;

            SUT.EndKey(configuredValue);

            _request.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            SUT.EndKey(configuredValue);

            _request.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_boolean_It_configures_underlying_options_Key()
        {
            const bool configuredValue = true;

            SUT.EndKey(configuredValue);

            _request.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_complex_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true
            };

            SUT.EndKey(configuredValue);

            _request.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Keys_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true,
                new object[] {"complex1", 42}
            };

            SUT.Keys(configuredValue);

            _request.Keys.Should().BeEquivalentTo(configuredValue);
        }
    }
}