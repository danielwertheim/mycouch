using FluentAssertions;
using MyCouch.Cloudant;
using MyCouch.Cloudant.Querying;
using Xunit;
using System.Linq;

namespace MyCouch.UnitTests.Cloudant
{
    public class IndexParametersConfiguratorTests : UnitTestsOf<IndexParametersConfigurator>
    {
        private readonly IIndexParameters _parameters;

        public IndexParametersConfiguratorTests()
        {
            _parameters = new IndexParameters();

            SUT = new IndexParametersConfigurator(_parameters);
        }

        [Fact]
        public void When_config_of_DesignDoc_It_configures_underlying_options_DesignDoc()
        {
            const string configuredValue = "some value";

            SUT.DesignDocument(configuredValue);

            _parameters.DesignDocument.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_Name_It_configures_underlying_options_Name()
        {
            const string configuredValue = "some value";

            SUT.Name(configuredValue);

            _parameters.Name.Should().Be(configuredValue);
        }

        [Fact]
        public void When_config_of_fields_it_configures_underlying_options_fields()
        {
            SUT.Fields(new IndexField("field1"), new IndexField("field2", SortDirection.Desc));

            _parameters.Fields.Count.Should().Be(2);
            _parameters.Fields.First().Name.Should().Be("field1");
            _parameters.Fields.First().SortDirection.Should().Be(SortDirection.Asc);
            _parameters.Fields.Last().Name.Should().Be("field2");
            _parameters.Fields.Last().SortDirection.Should().Be(SortDirection.Desc);
        }
    }
}
