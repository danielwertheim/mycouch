using System;
using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Querying;
using Xunit;

namespace MyCouch.UnitTests
{
    public class ShowConfigurationTests : UnitTestsOf<ShowParametersConfigurator>
    {
        private readonly IShowParameters _parameters;

        public ShowConfigurationTests()
        {
            _parameters = new ShowParameters(new ShowIdentity("foodesigndocument", "barshowname"));

            SUT = new ShowParametersConfigurator(_parameters);
        }

        [Fact]
        public void When_config_of_Id_It_configures_underlying_options_Id()
        {
            const string configuredValue = "myId";

            SUT.Id(configuredValue);

            _parameters.Id.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_AdditionalQueryParameters_It_configures_options_AdditionalQueryParameters()
        {
            var configuredValue = new Dictionary<string, object>
            {
                { "foo", new { bar = "fooobject" } },
                { "bar", "bar" }
            };

            SUT.CustomQueryParameters(configuredValue);

            _parameters.CustomQueryParameters.Should().Equal(configuredValue);
        }

        private enum FooEnum
        {
            One,
            Two
        }
    }
}