using FluentAssertions;
using MyCouch.Cloudant;
using MyCouch.Cloudant.Searching;
using Xunit;

namespace MyCouch.UnitTests.Cloudant
{
    public class SearchParametersConfiguratorTests : UnitTestsOf<SearchParametersConfigurator>
    {
        private readonly ISearchParameters _parameters;

        public SearchParametersConfiguratorTests()
        {
            _parameters = new SearchParameters(new SearchIndexIdentity("foodesigndocument", "barindexname"));

            SUT = new SearchParametersConfigurator(_parameters);
        }

        [Fact]
        public void When_config_of_Expression_of_string_It_configures_underlying_options_Expression()
        {
            const string configuredValue = "some value";

            SUT.Expression(configuredValue);

            _parameters.Expression.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Bookmark_of_string_It_configures_underlying_options_Bookmark()
        {
            const string configuredValue = "g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg";

            SUT.Bookmark(configuredValue);

            _parameters.Bookmark.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Sort_It_configures_underlying_options_Sort()
        {
            var configuredValue = new [] { "diet<string>", "latin_name<string>", "min_length<number>" };

            SUT.Sort(configuredValue);

            _parameters.Sort.Should().ContainInOrder(configuredValue);
        }

        [Fact]
        public void When_config_of_Sort_which_already_is_configures_It_uses_the_last_value_to_configure_underlying_options_Sort()
        {
            var configuredValue1 = new[] { "diet<string>", "latin_name<string>", "min_length<number>" };
            var configuredValue2 = new[] { "-diet<string>", "-latin_name<string>", "-min_length<number>" };

            SUT.Sort(configuredValue1);
            SUT.Sort(configuredValue2);

            _parameters.Sort.Should().ContainInOrder(configuredValue2);
        }

        [Fact]
        public void When_config_of_IncludeDocs_It_configures_underlying_options_IncludeDocs()
        {
            const bool configuredValue = true;

            SUT.IncludeDocs(configuredValue);

            _parameters.IncludeDocs.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Stale_It_configures_underlying_options_Stale()
        {
            var configuredValue = Stale.UpdateAfter;

            SUT.Stale(configuredValue);

            _parameters.Stale.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Limit_It_configures_underlying_options_Limit()
        {
            const int configuredValue = 10;

            SUT.Limit(configuredValue);

            _parameters.Limit.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_GroupField_It_configures_underlying_options_GroupField()
        {
            const string configuredValue = "name";

            SUT.GroupField(configuredValue);

            _parameters.GroupField.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_GroupLimit_It_configures_underlying_options_GroupLimit()
        {
            const int configuredValue = 10;

            SUT.GroupLimit(configuredValue);

            _parameters.GroupLimit.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_GroupSort_It_configures_underlying_options_GroupSort()
        {
            var configuredValue = new[] { "a", "b", "c" };

            SUT.GroupSort(configuredValue);

            _parameters.GroupSort.Should().ContainInOrder(configuredValue);
        }

        [Fact]
        public void When_config_of_Counts_It_configures_underlying_options_Counts()
        {
            var configuredValue = new[] { "a", "b", "c" };

            SUT.Counts(configuredValue);

            _parameters.Counts.Should().ContainInOrder(configuredValue);
        }

        [Fact]
        public void When_config_of_Ranges_It_configures_underlying_options_Ranges()
        {
            var configuredValue = new
            {
                min_length = new { minlight = "[0 TO 100]", minheavy = "{101 TO Infinity}" },
                max_length = new { maxlight = "[0 TO 100]", maxheavy = "{101 TO Infinity}" }
            };

            SUT.Ranges(configuredValue);

            _parameters.Ranges.Should().BeSameAs(configuredValue);
        }

        [Fact]
        public void When_config_of_DrillDown_It_configures_underlying_DrillDown()
        {
            var configuredfieldName = "configuredfieldName";
            var configuredfieldValue = "configuredfielValue";

            SUT.Drilldown(configuredfieldName, configuredfieldValue);

            _parameters.Drilldown.Should().NotBeNull();
            _parameters.Drilldown.Value.Key.Should().Be(configuredfieldName);
            _parameters.Drilldown.Value.Value.Should().Be(configuredfieldValue);
        }
    }
}