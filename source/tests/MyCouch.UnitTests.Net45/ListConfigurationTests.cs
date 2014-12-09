using System;
using FluentAssertions;
using MyCouch.Querying;
using Xunit;

namespace MyCouch.UnitTests
{
    public class ListConfigurationTests : UnitTestsOf<ListParametersConfigurator>
    {
        private readonly IListParameters _parameters;

        public ListConfigurationTests()
        {
            _parameters = new ListParameters(new ListIdentity("foodesigndocument", "barlistname"), "barviewname");

            SUT = new ListParametersConfigurator(_parameters);
        }

        [Fact]
        public void When_config_of_ViewName_It_configures_underlying_ViewNamw()
        {
            var viewName = "anotherviewname";

            SUT.ViewName(viewName);

            _parameters.ViewName.Should().Be(viewName);
        }
        /*
        [Fact]
        public void When_config_of_ViewQueryParameters_It_configures_underlying_ViewQueryParameters()
        {
            var configuredValue = new QueryParameters(new ViewIdentity("foodesigndocument", "barviewname"));

            SUT.ViewQueryParameters(configuredValue);

            _parameters.ViewQueryParameters.Should().Be(configuredValue);
        }

        [Fact]
        public void Should_propagate_attributes_of_ViewQueryParameters_to_the_underlying_ViewQueryParameters()
        {
            var configuredValue = new QueryParameters(new ViewIdentity("foodesigndocument", "barviewname"));
            var viewParametersConfigurer = new QueryParametersConfigurator(configuredValue);
            viewParametersConfigurer.Descending(true);

            SUT.ViewQueryParameters(configuredValue);

            _parameters.ViewQueryParameters.Descending.Should().Be(configuredValue.Descending);
        }
        */
    }
}