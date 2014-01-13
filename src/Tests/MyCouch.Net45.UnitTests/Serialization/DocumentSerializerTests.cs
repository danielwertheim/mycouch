using FluentAssertions;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class DocumentSerializerTests : SerializerTests<DefaultSerializer>
    {
        public DocumentSerializerTests()
        {
            SUT = new DocumentSerializer(new SerializationConfiguration(), new DocumentSerializationMetaProvider());
        }

        [Fact]
        public void When_deserializing_to_entity_with_Id_It_should_not_map_from__id()
        {
            var json = "{\"$doctype\":\"modelone\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelOne>(json);

            model.Should().NotBeNull();
            model.Id.Should().BeNull();
        }

        [Fact]
        public void When_deserializing_to_entity_with_EntityId_It_should_not_map_from__id()
        {
            var json = "{\"$doctype\":\"modeltwo\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelTwo>(json);

            model.Should().NotBeNull();
            model.EntityId.Should().BeNull();
        }

        [Fact]
        public void When_deserializing_to_entity_with_DocumentId_It_should_not_map_from__id()
        {
            var json = "{\"$doctype\":\"modelthree\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelThree>(json);

            model.Should().NotBeNull();
            model.DocumentId.Should().BeNull();
        }

        [Fact]
        public void When_deserializing_to_entity_with_ModelId_It_should_not_map_from__id()
        {
            var json = "{\"$doctype\":\"modelfour\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelFour>(json);

            model.Should().NotBeNull();
            model.ModelFourId.Should().BeNull();
        }

        [Fact]
        public void When_serializing_entity_It_will_inject_document_header_in_json()
        {
            var model = new ModelEntity { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"modelentity\"");
        }

        [Fact]
        public void When_serializing_entity_that_has_specific_docType_via_meta_It_will_use_that_as_document_header_in_json()
        {
            var model = new ModelEntityWithMeta { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"foo bar\"");
            json.Should().NotContain("\"$doctype\":\"modelentitywithmeta\"");
        }

        [Fact]
        public void When_serializing_child_extending_entity_that_has_specific_docType_via_meta_It_will_use_that_as_document_header_in_json()
        {
            var model = new ChildModelEntityWithMeta { Id = "abc", Rev = "505e07eb-41a4-4bb1-8a4c-fb6453f9927d", Value = "Some value." };

            var json = SUT.Serialize(model);

            json.Should().Contain("\"$doctype\":\"foo bar\"");
            json.Should().NotContain("\"$doctype\":\"childmodelentitywithmeta\"");
            json.Should().NotContain("\"$doctype\":\"modelentitywithmeta\"");
        }

        [Fact]
        public void When_serializing_entity_with_Id_It_will_not_translate_it_to__id()
        {
            var model = new ModelOne { Id = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().NotContain("\"_id\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_EntityId_It_will_not_translate_it_to__id()
        {
            var model = new ModelTwo { EntityId = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().NotContain("\"_id\":\"abc\"");
            json.Should().Contain("\"entityId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_DocumentId_It_will_not_translate_it_to__id()
        {
            var model = new ModelThree { DocumentId = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().NotContain("\"_id\":\"abc\"");
            json.Should().Contain("\"documentId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_ModelId_It_will_not_translate_it_to__id()
        {
            var model = new ModelFour { ModelFourId = "abc", Value = "def" };

            var json = SUT.Serialize(model);

            json.Should().NotContain("\"_id\":\"abc\"");
            json.Should().Contain("\"modelFourId\":\"abc\"");
        }

        [Fact]
        public void When_serializing_entity_with_Id_in_wrong_order_It_will_not_pick_any_as__id()
        {
            var model = new ModelWithIdInWrongOrder { Id = "abc", ModelWithIdInWrongOrderId = "def", Value = "ghi" };

            var json = SUT.Serialize(model);

            json.Should().NotContain("\"modelWithIdInWrongOrderId\":\"abc\"");
            json.Should().Contain("\"id\":\"abc\"");
        }
    }
}