using FluentAssertions;
using MyCouch.Serialization;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public abstract class SerializerTests<T> : UnitTestsOf<T> where T : class, ISerializer
    {
        [Fact]
        public void It_can_deserialize_entity_with_Id()
        {
            var json = "{\"$doctype\":\"modelone\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelOne>(json);

            model.Should().NotBeNull();
            model.Id.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Fact]
        public void It_can_deserialize_entity_with_EntityId()
        {
            var json = "{\"$doctype\":\"modeltwo\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelTwo>(json);

            model.Should().NotBeNull();
            model.EntityId.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Fact]
        public void It_can_deserialize_entity_with_DocumentId()
        {
            var json = "{\"$doctype\":\"modelthree\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelThree>(json);

            model.Should().NotBeNull();
            model.DocumentId.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        [Fact]
        public void It_can_deserialize_entity_with_ModelId()
        {
            var json = "{\"$doctype\":\"modelfour\",\"_id\":\"abc\",\"value\":\"def\"}";

            var model = SUT.Deserialize<ModelFour>(json);

            model.Should().NotBeNull();
            model.ModelFourId.Should().Be("abc");
            model.Value.Should().Be("def");
        }

        protected class ModelWithIdInWrongOrder
        {
            public string Id { get; set; }
            public string ModelWithIdInWrongOrderId { get; set; }
            public string Value { get; set; }
        }

        protected class ModelOne
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        protected class ModelTwo
        {
            public string EntityId { get; set; }
            public string Value { get; set; }
        }

        protected class ModelThree
        {
            public string DocumentId { get; set; }
            public string Value { get; set; }
        }

        protected class ModelFour
        {
            public string ModelFourId { get; set; }
            public string Value { get; set; }
        }
    }
}
