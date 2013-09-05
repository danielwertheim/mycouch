using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class EntitySerializationWithLambdaPropertyFactoryTests : EntitySerializationTests
    {
        public EntitySerializationWithLambdaPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
            SUT = new EntitySerializer(CreateSerializationConfiguration(entityReflector));
        }
    }
#if !NETFX_CORE
    public class EntitySerializationWithIlPropertyFactoryTests : EntitySerializationTests
    {
        public EntitySerializationWithIlPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            SUT = new EntitySerializer(CreateSerializationConfiguration(entityReflector));
        }
    }
#endif

    public abstract class EntitySerializationTests : SerializerTests<DefaultSerializer>
    {
        protected SerializationConfiguration CreateSerializationConfiguration(EntityReflector entityReflector)
        {
            return new SerializationConfiguration(new EntityContractResolver(entityReflector));
        }

        [Fact]
        public void When_deserializing_to_entity_with_Id_It_should_map_from__id()
        {
            var json = "{\"$doctype\":\"modelone\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelOne>(json);

            model.Should().NotBeNull();
            model.Id.Should().Be("abc");
        }

        [Fact]
        public void When_deserializing_to_entity_with_EntityId_It_should_map_from__id()
        {
            var json = "{\"$doctype\":\"modeltwo\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelTwo>(json);

            model.Should().NotBeNull();
            model.EntityId.Should().Be("abc");
        }

        [Fact]
        public void When_deserializing_to_entity_with_DocumentId_It_should_map_from__id()
        {
            var json = "{\"$doctype\":\"modelthree\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelThree>(json);

            model.Should().NotBeNull();
            model.DocumentId.Should().Be("abc");
        }

        [Fact]
        public void When_deserializing_to_entity_with_ModelId_It_should_map_from__id()
        {
            var json = "{\"$doctype\":\"modelfour\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelFour>(json);

            model.Should().NotBeNull();
            model.ModelFourId.Should().Be("abc");
        }

        [Fact]
        public void When_serializing_entity_It_will_inject_document_header_in_json()
        {
            var model = ClientTestData.Artists.CreateArtist();

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"artist\"");
        }

        [Fact]
        public void When_serializing_entity_with_Id_It_will_translate_it_to__id()
        {
            var model = new ModelOne { Id = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"_id\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_EntityId_It_will_translate_it_to__id()
        {
            var model = new ModelTwo { EntityId = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"_id\":\"abc\"");
            json.Should().NotContain("\"entityId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_DocumentId_It_will_translate_it_to__id()
        {
            var model = new ModelThree { DocumentId = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"_id\":\"abc\"");
            json.Should().NotContain("\"documentId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_ModelId_It_will_translate_it_to__id()
        {
            var model = new ModelFour { ModelFourId = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"_id\":\"abc\"");
            json.Should().NotContain("\"modelFourId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_Id_in_wrong_order_It_will_still_pick_the_more_specific_one()
        {
            var model = new ModelWithIdInWrongOrder { Id = "abc", ModelWithIdInWrongOrderId = "def", Value = "ghi" };

            var json = SUT.Serialize(model);

            json.Should().Be("{\"$doctype\":\"modelwithidinwrongorder\",\"id\":\"abc\",\"_id\":\"def\",\"value\":\"ghi\"}");
        }
    }
}