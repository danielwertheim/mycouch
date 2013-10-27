using FluentAssertions;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Requests.Configurators;
using Xunit;

namespace MyCouch.UnitTests.Cloudant.Requests
{
    public class SearchIndexRequestConfiguratorTests : UnitTestsOf<SearchIndexRequestConfigurator>
    {
        private readonly SearchIndexRequest _request;

        public SearchIndexRequestConfiguratorTests()
        {
            _request = new SearchIndexRequest("foodesigndocument", "barindexname");

            SUT = new SearchIndexRequestConfigurator(_request);
        }

        [Fact]
        public void When_config_of_Expression_of_string_It_configures_underlying_options_Expression()
        {
            const string configuredValue = "some value";

            SUT.Expression(configuredValue);

            _request.Expression.Should().Be(configuredValue);
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
    }
}