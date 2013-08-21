using FluentAssertions;
using MyCouch.Rich.Schemes;
using MyCouch.Rich.Schemes.Reflections;
using MyCouch.Rich.Serialization;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class RichSerializerWithLambdaPropertyFactoryTests : RichSerializerTests
    {
        public RichSerializerWithLambdaPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
            SUT = new RichSerializer(new RichSerializationContractResolver(entityReflector));
        }
    }
#if !NETFX_CORE
    public class RichSerializerWithIlPropertyFactoryTests : RichSerializerTests
    {
        public RichSerializerWithIlPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            SUT = new RichSerializer(new RichSerializationContractResolver(entityReflector));
        }
    }
#endif

    public abstract class RichSerializerTests : SerializerTests<RichSerializer>
    {
        [Fact]
        public void When_serializing_entity_It_will_inject_document_header_in_json()
        {
            var model = TestData.Artists.CreateArtist();

            var json = SUT.SerializeEntity(model);

            json.Should().Contain("\"$doctype\":\"artist\"");
        }

        [Fact]
        public void When_serializing_entity_with_Id_It_will_translate_it_to__id()
        {
            var model = new ModelOne { Id = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Contain("\"_id\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_EntityId_It_will_translate_it_to__id()
        {
            var model = new ModelTwo { EntityId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Contain("\"_id\":\"abc\"");
            json.Should().NotContain("\"entityId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_DocumentId_It_will_translate_it_to__id()
        {
            var model = new ModelThree { DocumentId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Contain("\"_id\":\"abc\"");
            json.Should().NotContain("\"documentId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_ModelId_It_will_translate_it_to__id()
        {
            var model = new ModelFour { ModelFourId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Contain("\"_id\":\"abc\"");
            json.Should().NotContain("\"modelFourId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_Id_in_wrong_order_It_will_still_pick_the_more_specific_one()
        {
            var model = new ModelWithIdInWrongOrder { Id = "abc", ModelWithIdInWrongOrderId = "def", Value = "ghi" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelwithidinwrongorder\",\"id\":\"abc\",\"_id\":\"def\",\"value\":\"ghi\"}");
        }
    }
}