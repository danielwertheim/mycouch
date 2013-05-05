using FluentAssertions;
using NUnit.Framework;

namespace MyCouch.UnitTests
{
    [TestFixture]
    public class BulkCommandTests : UnitTestsOf<BulkCommand>
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = new BulkCommand();
        }

        [Test]
        public void When_empty_Then_ToJson_returns_empty_array_document()
        {
            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[]}");
        }

        [Test]
        public void When_it_has_deletes_exists_Then_ToJson_returns_deletes()
        {
            SUT.Delete("1", "1-1").Delete("2", "2-1");
            
            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[{\"_id\":\"1\",\"_rev\":\"1-1\",\"_deleted\":true},{\"_id\":\"2\",\"_rev\":\"2-1\",\"_deleted\":true}]}");
        }

        [Test]
        public void When_it_has_inlcudes_of_put_and_posts_Then_ToJson_returns_docs()
        {
            SUT.Include(
                "{\"_id\":\"1\",\"name\":\"Test1\"}", 
                "{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}");

            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[{\"_id\":\"1\",\"name\":\"Test1\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}]}");
        }

        [Test]
        public void When_it_has_inlcudes_of_put_and_posts_and_deletes_Then_ToJson_returns_docs()
        {
            SUT.Include(
                "{\"_id\":\"1\",\"name\":\"Test1\"}",
                "{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}")
                .Delete("1", "1-1")
                .Delete("2", "2-1");

            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[{\"_id\":\"1\",\"name\":\"Test1\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"_deleted\":true},{\"_id\":\"2\",\"_rev\":\"2-1\",\"_deleted\":true}]}");
        }
    }
}