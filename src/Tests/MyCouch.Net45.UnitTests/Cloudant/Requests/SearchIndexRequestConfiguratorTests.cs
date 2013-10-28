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
        public void When_config_of_Bookmark_of_string_It_configures_underlying_options_Bookmark()
        {
            const string configuredValue = "g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg";

            SUT.Bookmark(configuredValue);

            _request.Bookmark.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Sort_It_configures_underlying_options_Sort()
        {
            var configuredValue = new [] { "diet<string>", "latin_name<string>", "min_length<number>" };

            SUT.Sort(configuredValue);

            _request.Sort.Should().ContainInOrder(configuredValue);
        }

        [Fact]
        public void When_config_of_Sort_which_already_is_configures_It_uses_the_last_value_to_configure_underlying_options_Sort()
        {
            var configuredValue1 = new[] { "diet<string>", "latin_name<string>", "min_length<number>" };
            var configuredValue2 = new[] { "-diet<string>", "-latin_name<string>", "-min_length<number>" };

            SUT.Sort(configuredValue1);
            SUT.Sort(configuredValue2);

            _request.Sort.Should().ContainInOrder(configuredValue2);
        }

        [Fact]
        public void When_config_of_IncludeDocs_It_configures_underlying_options_IncludeDocs()
        {
            const bool configuredValue = true;

            SUT.IncludeDocs(configuredValue);

            _request.IncludeDocs.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Stale_It_configures_underlying_options_Stale()
        {
            var configuredValue = Stale.UpdateAfter;

            SUT.Stale(configuredValue);

            _request.Stale.Should().Be(configuredValue);
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