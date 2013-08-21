using FluentAssertions;
using MyCouch.Serialization;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class DefaultSerializerTests : SerializerTests<DefaultSerializer>
    {
        public DefaultSerializerTests()
        {
            SUT = new DefaultSerializer();
        }

        [Fact]
        public void When_serializing_entity_It_will_not_inject_document_header_in_json()
        {
            var model = TestData.Artists.CreateArtist();

            var json = SUT.Serialize(model);

            json.Should().NotContain("\"$doctype\":\"artist\"");
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

            json.Should().Be("{\"$doctype\":\"modelwithidinwrongorder\",\"id\":\"abc\",\"modelWithIdInWrongOrderId\":\"def\",\"value\":\"ghi\"}");
        }
    }
}