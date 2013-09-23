using MyCouch.Serialization;

namespace MyCouch.UnitTests.Serialization
{
    public abstract class SerializerTests<T> : UnitTestsOf<T> where T : class, ISerializer
    {
        protected class ModelEntity
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Value { get; set; }
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
