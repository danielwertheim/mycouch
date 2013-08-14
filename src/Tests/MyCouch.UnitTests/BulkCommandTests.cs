using FluentAssertions;
using MyCouch.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCouch.UnitTests
{
    [TestClass]
    public class BulkCommandTests : UnitTestsOf<BulkCommand>
    {
        public BulkCommandTests()
        {
            OnTestInitialize = () => SUT = new BulkCommand();
        }

        [TestMethod]
        public void When_empty_Then_ToJson_returns_empty_array_document()
        {
            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[]}");
        }

        [TestMethod]
        public void When_it_has_deletes_exists_Then_ToJson_returns_deletes()
        {
            SUT.Delete("1", "1-1").Delete("2", "2-1");
            
            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[{\"_id\":\"1\",\"_rev\":\"1-1\",\"_deleted\":true},{\"_id\":\"2\",\"_rev\":\"2-1\",\"_deleted\":true}]}");
        }

        [TestMethod]
        public void When_it_has_inlcudes_of_put_and_posts_Then_ToJson_returns_docs()
        {
            SUT.Include(
                "{\"_id\":\"1\",\"name\":\"Test1\"}", 
                "{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}");

            var json = SUT.ToJson();
            json.Should().Be("{\"docs\":[{\"_id\":\"1\",\"name\":\"Test1\"},{\"_id\":\"1\",\"_rev\":\"1-1\",\"name\":\"Test2\"}]}");
        }

        [TestMethod]
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