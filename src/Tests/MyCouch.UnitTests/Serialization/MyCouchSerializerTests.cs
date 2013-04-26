using FluentAssertions;
using MyCouch.Schemes;
using MyCouch.Serialization;
using MyCouch.Testing;
using NUnit.Framework;

namespace MyCouch.UnitTests.Serialization
{
    [TestFixture]
    public class MyCouchSerializerTests : UnitTestsOf<MyCouchSerializer>
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = new MyCouchSerializer(new EntityAccessor());
        }

        [Test]
        public void When_serializing_entity_It_will_inject_document_header_in_json()
        {
            var model = TestData.CreateArtist();

            var json = SUT.SerializeEntity(model);

            json.Should().StartWith("{\"$doctype\":\"artist\",");
        }

        [Test]
        public void When_serializing_entity_with_Id_It_will_translate_it_to__id()
        {
            var model = new ModelOne { Id = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelone\",\"_id\":\"abc\",\"value\":\"def\"}");
        }

        [Test]
        public void It_can_deserialize_entity_with_Id()
        {
            var json = "{\"$doctype\":\"modelone\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelOne>(json);

            model.Should().NotBeNull();
            model.Id.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Test]
        public void When_serializing_entity_with_EntityId_It_will_translate_it_to__id()
        {
            var model = new ModelTwo { EntityId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modeltwo\",\"_id\":\"abc\",\"value\":\"def\"}");
        }

        [Test]
        public void It_can_deserialize_entity_with_EntityId()
        {
            var json = "{\"$doctype\":\"modeltwo\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelTwo>(json);

            model.Should().NotBeNull();
            model.EntityId.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Test]
        public void When_serializing_entity_with_DocumentId_It_will_translate_it_to__id()
        {
            var model = new ModelThree { DocumentId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelthree\",\"_id\":\"abc\",\"value\":\"def\"}");
        }

        [Test]
        public void It_can_deserialize_entity_with_DocumentId()
        {
            var json = "{\"$doctype\":\"modelthree\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelThree>(json);

            model.Should().NotBeNull();
            model.DocumentId.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Test]
        public void When_serializing_entity_with_ModelId_It_will_translate_it_to__id()
        {
            var model = new ModelFour { ModelFourId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelfour\",\"_id\":\"abc\",\"value\":\"def\"}");
        }

        [Test]
        public void It_can_deserialize_entity_with_ModelId()
        {
            var json = "{\"$doctype\":\"modelfour\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelFour>(json);

            model.Should().NotBeNull();
            model.ModelFourId.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Test]
        public void When_serializing_entity_with_Id_in_wrong_order_It_will_still_pick_the_more_specific_one()
        {
            var model = new ModelWithIdInWrongOrder { Id = "abc", ModelWithIdInWrongOrderId = "def", Value = "ghi" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelwithidinwrongorder\",\"id\":\"abc\",\"_id\":\"def\",\"value\":\"ghi\"}");
        }

        private class ModelWithIdInWrongOrder
        {
            public string Id { get; set; }
            public string ModelWithIdInWrongOrderId { get; set; }
            public string Value { get; set; }
        }

        private class ModelOne
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        private class ModelTwo
        {
            public string EntityId { get; set; }
            public string Value { get; set; }
        }

        private class ModelThree
        {
            public string DocumentId { get; set; }
            public string Value { get; set; }
        }

        private class ModelFour
        {
            public string ModelFourId { get; set; }
            public string Value { get; set; }
        }
    }
}