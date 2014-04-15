using System;
using FluentAssertions;
using MyCouch.Querying;
using Xunit;

namespace MyCouch.UnitTests
{
    public class QueryConfigurationTests : UnitTestsOf<QueryParametersConfigurator>
    {
        private readonly IQueryParameters _parameters;

        public QueryConfigurationTests()
        {
            _parameters = new QueryParameters(new ViewIdentity("foodesigndocument", "barviewname"));

            SUT = new QueryParametersConfigurator(_parameters);
        }

        [Fact]
        public void When_config_of_Descending_It_configures_underlying_options_Descending()
        {
            const bool configuredValue = true;

            SUT.Descending(configuredValue);

            _parameters.Descending.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_IncludeDocs_It_configures_underlying_options_IncludeDocs()
        {
            const bool configuredValue = true;

            SUT.IncludeDocs(configuredValue);

            _parameters.IncludeDocs.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Skip_It_configures_underlying_options_Skip()
        {
            const int configuredValue = 10;

            SUT.Skip(configuredValue);

            _parameters.Skip.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Limit_It_configures_underlying_options_Limit()
        {
            const int configuredValue = 10;

            SUT.Limit(configuredValue);

            _parameters.Limit.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_string_It_configures_underlying_options_Key()
        {
            const string configuredValue = "Fake key 1";

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_enum_It_configures_underlying_options_Key()
        {
            const FooEnum configuredValue = FooEnum.Two;

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_int_It_configures_underlying_options_Key()
        {
            const int configuredValue = 42;

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_double_It_configures_underlying_options_Key()
        {
            const double configuredValue = 3.14;

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_boolean_It_configures_underlying_options_Key()
        {
            const bool configuredValue = true;

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_complex_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                FooEnum.Two,
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true
            };

            SUT.Key(configuredValue);

            _parameters.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_string_It_configures_underlying_options_Key()
        {
            const string configuredValue = "Fake key 1";

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_enum_It_configures_underlying_options_Key()
        {
            const FooEnum configuredValue = FooEnum.Two;

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_int_It_configures_underlying_options_Key()
        {
            const int configuredValue = 42;

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_double_It_configures_underlying_options_Key()
        {
            const double configuredValue = 3.14;

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_boolean_It_configures_underlying_options_Key()
        {
            const bool configuredValue = true;

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_StartKey_of_complex_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                FooEnum.Two,
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true
            };

            SUT.StartKey(configuredValue);

            _parameters.StartKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_string_It_configures_underlying_options_Key()
        {
            const string configuredValue = "Fake key 1";

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_enum_It_configures_underlying_options_Key()
        {
            const FooEnum configuredValue = FooEnum.Two;

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_int_It_configures_underlying_options_Key()
        {
            const int configuredValue = 42;

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_double_It_configures_underlying_options_Key()
        {
            const double configuredValue = 3.14;

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008, 07, 17, 09, 21, 30, 50);

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_boolean_It_configures_underlying_options_Key()
        {
            const bool configuredValue = true;

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_EndKey_of_complex_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                FooEnum.Two,
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true
            };

            SUT.EndKey(configuredValue);

            _parameters.EndKey.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Keys_It_configures_underlying_options_Key()
        {
            var configuredValue = new object[] {
                "fake key",
                FooEnum.Two,
                42,
                3.14,
                new DateTime(2008, 07, 17, 09, 21, 30, 50),
                true,
                new object[] {"complex1", 42}
            };

            SUT.Keys(configuredValue);

            _parameters.Keys.Should().BeEquivalentTo(configuredValue);
        }

        [Fact]
        public void When_config_of_Stale_It_configures_underlying_options_Stale()
        {
            var configuredValue = Stale.UpdateAfter;

            SUT.Stale(configuredValue);

            _parameters.Stale.Should().Be(configuredValue);
        }

        private enum FooEnum
        {
            One,
            Two
        }
    }
}