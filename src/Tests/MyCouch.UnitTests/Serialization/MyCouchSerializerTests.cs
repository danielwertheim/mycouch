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
            var model = TestDataFactory.CreateArtist();

            var json = SUT.SerializeEntity(model);

            json.Should().Contain("\"$doctype\":\"artist\"");
        }
    }
}