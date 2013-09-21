using System;
using FluentAssertions;
using MyCouch.Querying;
using Xunit;

namespace MyCouch.UnitTests.Querying
{
    public class ViewQueryConfiguratorTests : UnitTestsOf<ViewQueryConfigurator>
    {
        private readonly ViewQueryOptions _viewQueryOptions;

        public ViewQueryConfiguratorTests()
        {
            _viewQueryOptions = new ViewQueryOptions();

            SUT = new ViewQueryConfigurator(_viewQueryOptions);
        }

        [Fact]
        public void When_config_of_Key_of_string_It_configures_underlying_options_Key()
        {
            var configuredValue = "Fake key 1";

            SUT.Key(configuredValue);

            _viewQueryOptions.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_int_It_configures_underlying_options_Key()
        {
            var configuredValue = 42;

            SUT.Key(configuredValue);

            _viewQueryOptions.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_double_It_configures_underlying_options_Key()
        {
            var configuredValue = 3.14;

            SUT.Key(configuredValue);

            _viewQueryOptions.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_date_It_configures_underlying_options_Key()
        {
            var configuredValue = new DateTime(2008,07,17,09, 21, 30, 50);

            SUT.Key(configuredValue);

            _viewQueryOptions.Key.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Key_of_boolean_It_configures_underlying_options_Key()
        {
            var configuredValue = true;

            SUT.Key(configuredValue);

            _viewQueryOptions.Key.Should().Be(configuredValue);
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

            _viewQueryOptions.Key.Should().Be(configuredValue);
        }
    }
}