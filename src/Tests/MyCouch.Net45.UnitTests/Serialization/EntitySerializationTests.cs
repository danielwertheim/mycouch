using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class EntitySerializationWithLambdaPropertyFactoryTests : EntitySerializationTests
    {
        public EntitySerializationWithLambdaPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
            SUT = new EntitySerializer(CreateSerializationConfiguration(entityReflector), CreateDocumentSerializationMetaProvider());
        }
    }
#if !NETFX_CORE
    public class EntitySerializationWithIlPropertyFactoryTests : EntitySerializationTests
    {
        public EntitySerializationWithIlPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            SUT = new EntitySerializer(CreateSerializationConfiguration(entityReflector), CreateDocumentSerializationMetaProvider());
        }
    }
#endif

    public abstract class EntitySerializationTests : SerializerTests<EntitySerializer>
    {
        protected static SerializationConfiguration CreateSerializationConfiguration(EntityReflector entityReflector)
        {
            return new SerializationConfiguration(new EntityContractResolver(entityReflector));
        }

        protected static IDocumentSerializationMetaProvider CreateDocumentSerializationMetaProvider()
        {
            return new DocumentSerializationMetaProvider();
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
        public void When_serializing_entity_It_will_inject_docType_in_json()
        {
            var model = new ModelEntity { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"modelentity\"");
        }

        [Fact]
        public void When_serializing_child_entity_It_will_inject_child_docType_json()
        {
            var model = new ChildModelEntity { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"childmodelentity\"");
        }

        [Fact]
        public void When_serializing_entity_with_specific_DocType_via_meta_It_will_use_that_in_json()
        {
            var model = new ModelEntityWithMeta { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"foo bar\"");
            json.Should().NotContain("\"$doctype\":\"modelentitywithmeta\"");
        }

        [Fact]
        public void When_serializing_entity_with_specific_DocNamespace_via_meta_It_will_use_that_in_json()
        {
            var model = new ModelEntityWithMeta { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$docns\":\"MyNs\"");
        }

        [Fact]
        public void When_serializing_entity_with_specific_DocVersion_via_meta_It_will_use_that_in_json()
        {
            var model = new ModelEntityWithMeta { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$docver\":\"1.2.1\"");
        }

        [Fact]
        public void When_serializing_child_extending_entity_that_has_specific_docType_via_meta_It_will_use_that_in_json()
        {
            var model = new ChildModelEntityWithMeta { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"foo bar\"");
            json.Should().NotContain("\"$doctype\":\"childmodelentitywithmeta\"");
            json.Should().NotContain("\"$doctype\":\"modelentitywithmeta\"");
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