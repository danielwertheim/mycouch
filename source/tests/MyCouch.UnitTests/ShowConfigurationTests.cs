using System;
using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Querying;
using Xunit;

namespace MyCouch.UnitTests
{
    public class ShowConfigurationTests : UnitTestsOf<QueryShowParametersConfigurator>
    {
        private readonly IShowParameters _parameters;

        public ShowConfigurationTests()
        {
            _parameters = new ShowParameters(new ShowIdentity("foodesigndocument", "barshowname"));

            SUT = new QueryShowParametersConfigurator(_parameters);
        }

        [Fact]
        public void When_config_of_DocId_It_configures_underlying_options_DocId()
        {
            const string configuredValue = "myId";

            SUT.DocId(configuredValue);

            _parameters.DocId.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Accepts_It_configures_options_Accepts()
        {
            var configuredValue = new[] { "foo", "bar" };

            SUT.Accepts(configuredValue);

            _parameters.Accepts.Should().Equal(configuredValue);
        }

        [Fact]
        public void When_config_of_CustomQueryParameters_It_configures_options_CustomQueryParameters()
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