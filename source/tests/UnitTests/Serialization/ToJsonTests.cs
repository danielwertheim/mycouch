using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;
using Xunit;

namespace UnitTests.Serialization
{
    public class ToJsonWithLambdaPropertyFactoryTests : ToJsonTests
    {
        public ToJsonWithLambdaPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
            var configuration = new SerializationConfiguration(new SerializationContractResolver());
            SUT = new DefaultSerializer(configuration, new DocumentSerializationMetaProvider(), entityReflector);
        }
    }

    public class ToJsonWithIlPropertyFactoryTests : ToJsonTests
    {
        public ToJsonWithIlPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            var configuration = new SerializationConfiguration(new SerializationContractResolver());
            SUT = new DefaultSerializer(configuration, new DocumentSerializationMetaProvider(), entityReflector);
        }
    }

    public abstract class ToJsonTests : UnitTestsOf<DefaultSerializer>
    {
        [Theory]
        [InlineData("Test\"data", "\"Test\\\"data\"")]
        [InlineData("Test\\data", "\"Test\\\\data\"")]
        [InlineData("Test\\\\data", "\"Test\\\\\\\\data\"")]
        public void ToJson_String_escapes_its_data(string input, string expected)
        {
            string serialized = SUT.ToJson(input);

            serialized.Should().Be(expected);
        }

        [Theory]
        [InlineData(new[] { "Test\"data" }, "[\"Test\\\"data\"]")]
        [InlineData(new[] { "Test\\data" }, "[\"Test\\\\data\"]")]
        [InlineData(new[] { "Test\\\\data" }, "[\"Test\\\\\\\\data\"]")]
        [InlineData(new[] { "Test\"data", "Test\\data" }, "[\"Test\\\"data\",\"Test\\\\data\"]")]
        [InlineData(new[] { "Test\\\\data", "Test\"data", "Test\\data" }, "[\"Test\\\\\\\\data\",\"Test\\\"data\",\"Test\\\\data\"]")]
        public void ToJson_String_array_escapes_its_data(string[] inputs, string expected)
        {
            string serialized = SUT.ToJson(inputs);

            serialized.Should().Be(expected);
        }
    }
}
