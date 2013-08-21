using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Schemes;
using MyCouch.Schemes.Reflections;
using MyCouch.Serialization;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class MyCouchSerializerWithLambdaPropertyFactoryTests : MyCouchSerializerTests
    {
        public MyCouchSerializerWithLambdaPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
            SUT = new DefaultSerializer(() => entityReflector);
        }
    }
#if !NETFX_CORE
    public class MyCouchSerializerWithIlPropertyFactoryTests : MyCouchSerializerTests
    {
        public MyCouchSerializerWithIlPropertyFactoryTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            SUT = new DefaultSerializer(() => entityReflector);
        }
    }
#endif
    public abstract class MyCouchSerializerTests : UnitTestsOf<DefaultSerializer>
    {
        [Fact]
        public void When_serializing_entity_It_will_inject_document_header_in_json()
        {
            var model = TestData.Artists.CreateArtist();

            var json = SUT.SerializeEntity(model);

            json.Should().StartWith("{\"$doctype\":\"artist\",");
        }

        [Fact]
        public void When_serializing_entity_with_Id_It_will_translate_it_to__id()
        {
            var model = new ModelOne { Id = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelone\",\"_id\":\"abc\",\"value\":\"def\"}");
        }

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
        public void When_serializing_entity_with_EntityId_It_will_translate_it_to__id()
        {
            var model = new ModelTwo { EntityId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modeltwo\",\"_id\":\"abc\",\"value\":\"def\"}");
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
        public void When_serializing_entity_with_DocumentId_It_will_translate_it_to__id()
        {
            var model = new ModelThree { DocumentId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelthree\",\"_id\":\"abc\",\"value\":\"def\"}");
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
        public void When_serializing_entity_with_ModelId_It_will_translate_it_to__id()
        {
            var model = new ModelFour { ModelFourId = "abc", Value = "def" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelfour\",\"_id\":\"abc\",\"value\":\"def\"}");
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

        [Fact]
        public void When_serializing_entity_with_Id_in_wrong_order_It_will_still_pick_the_more_specific_one()
        {
            var model = new ModelWithIdInWrongOrder { Id = "abc", ModelWithIdInWrongOrderId = "def", Value = "ghi" };

            var json = SUT.SerializeEntity(model);

            json.Should().Be("{\"$doctype\":\"modelwithidinwrongorder\",\"id\":\"abc\",\"_id\":\"def\",\"value\":\"ghi\"}");
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_string()
        {
            var response = new ViewQueryResponse<string>();
            
            SUT.PopulateViewQueryResponse(response, JsonTestData.ViewQueryAllDocsResult.AsStream());

            response.RowCount.Should().Be(2);
            response.Rows[0].Id.Should().Be("1");
            response.Rows[0].Key.Should().Be("1");
            response.Rows[0].Value.Should().Be("{\"rev\":\"43-4886b6a3da60a647adea18b1c6c81cd5\"}");

            response.Rows[1].Id.Should().Be("2");
            response.Rows[1].Key.Should().Be("2");
            response.Rows[1].Value.Should().Be("{\"rev\":\"42-e7620ba0ea71c48f6a11bacee4999d79\"}");
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_dynamic()
        {
            var response = new ViewQueryResponse<dynamic>();

            SUT.PopulateViewQueryResponse(response, JsonTestData.ViewQueryAllDocsResult.AsStream());

            response.RowCount.Should().Be(2);
            response.Rows[0].Id.Should().Be("1");
            response.Rows[0].Key.Should().Be("1");
            string rev1 = response.Rows[0].Value.rev;
            rev1.Should().Be("43-4886b6a3da60a647adea18b1c6c81cd5");

            response.Rows[1].Id.Should().Be("2");
            response.Rows[1].Key.Should().Be("2");
            string rev2 = response.Rows[1].Value.rev;
            rev2.Should().Be("42-e7620ba0ea71c48f6a11bacee4999d79");
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_dictionary()
        {
            var response = new ViewQueryResponse<IDictionary<string, object>>();

            SUT.PopulateViewQueryResponse(response, JsonTestData.ViewQueryAllDocsResult.AsStream());

            response.RowCount.Should().Be(2);
            response.Rows[0].Id.Should().Be("1");
            response.Rows[0].Key.Should().Be("1");
            string rev1 = response.Rows[0].Value["rev"].ToString();
            rev1.Should().Be("43-4886b6a3da60a647adea18b1c6c81cd5");

            response.Rows[1].Id.Should().Be("2");
            response.Rows[1].Key.Should().Be("2");
            string rev2 = response.Rows[1].Value["rev"].ToString();
            rev2.Should().Be("42-e7620ba0ea71c48f6a11bacee4999d79");
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_string()
        {
            var response = new ViewQueryResponse<string>();

            SUT.PopulateViewQueryResponse(response, JsonTestData.ViewQueryAlbums.AsStream());

            response.RowCount.Should().Be(4);
            response.Rows[0].Id.Should().Be("1");
            response.Rows[0].Key.Should().Be("Fake artist 1");
            response.Rows[0].Value.Should().Be("[{\"name\":\"Greatest fakes #1\"}]");

            response.Rows[1].Id.Should().Be("2");
            response.Rows[1].Key.Should().Be("Fake artist 2");
            response.Rows[1].Value.Should().Be("[{\"name\":\"Greatest fakes #2.1\"},{\"name\":\"Greatest fakes #2.2\"}]");

            response.Rows[2].Id.Should().Be("3");
            response.Rows[2].Key.Should().Be("Fake artist 3");
            response.Rows[2].Value.Should().Be("[{\"name\":\"Greatest fakes #3.1\"},{\"name\":\"Greatest fakes #3.2\"},{\"name\":\"Greatest fakes #3.3\"}]");

            response.Rows[3].Id.Should().Be("4");
            response.Rows[3].Key.Should().Be("Fake artist 4");
            response.Rows[3].Value.Should().Be("[{\"name\":\"Greatest fakes #4.1\"},{\"name\":\"Greatest fakes #4.2\"},{\"name\":\"Greatest fakes #4.3\"},{\"name\":\"Greatest fakes #4.4\"}]");
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_strings()
        {
            var response = new ViewQueryResponse<string[]>();

            SUT.PopulateViewQueryResponse(response, JsonTestData.ViewQueryAlbums.AsStream());

            response.RowCount.Should().Be(4);
            response.Rows[0].Id.Should().Be("1");
            response.Rows[0].Key.Should().Be("Fake artist 1");
            response.Rows[0].Value.Length.Should().Be(1);
            response.Rows[0].Value[0].Should().Be("{\"name\":\"Greatest fakes #1\"}");

            response.Rows[1].Id.Should().Be("2");
            response.Rows[1].Key.Should().Be("Fake artist 2");
            response.Rows[1].Value.Length.Should().Be(2);
            response.Rows[1].Value[0].Should().Be("{\"name\":\"Greatest fakes #2.1\"}");
            response.Rows[1].Value[1].Should().Be("{\"name\":\"Greatest fakes #2.2\"}");

            response.Rows[2].Id.Should().Be("3");
            response.Rows[2].Key.Should().Be("Fake artist 3");
            response.Rows[2].Value.Length.Should().Be(3);
            response.Rows[2].Value[0].Should().Be("{\"name\":\"Greatest fakes #3.1\"}");
            response.Rows[2].Value[1].Should().Be("{\"name\":\"Greatest fakes #3.2\"}");
            response.Rows[2].Value[2].Should().Be("{\"name\":\"Greatest fakes #3.3\"}");

            response.Rows[3].Id.Should().Be("4");
            response.Rows[3].Key.Should().Be("Fake artist 4");
            response.Rows[3].Value.Length.Should().Be(4);
            response.Rows[3].Value[0].Should().Be("{\"name\":\"Greatest fakes #4.1\"}");
            response.Rows[3].Value[1].Should().Be("{\"name\":\"Greatest fakes #4.2\"}");
            response.Rows[3].Value[2].Should().Be("{\"name\":\"Greatest fakes #4.3\"}");
            response.Rows[3].Value[3].Should().Be("{\"name\":\"Greatest fakes #4.4\"}");
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_albums()
        {
            var response = new ViewQueryResponse<Album[]>();

            SUT.PopulateViewQueryResponse(response, JsonTestData.ViewQueryAlbums.AsStream());

            response.RowCount.Should().Be(4);
            response.Rows[0].Id.Should().Be("1");
            response.Rows[0].Key.Should().Be("Fake artist 1");
            response.Rows[0].Value.Length.Should().Be(1);
            CustomAsserts.AreValueEqual(TestData.Artists.Artist1.Albums, response.Rows[0].Value);

            response.Rows[1].Id.Should().Be("2");
            response.Rows[1].Key.Should().Be("Fake artist 2");
            response.Rows[1].Value.Length.Should().Be(2);
            CustomAsserts.AreValueEqual(TestData.Artists.Artist2.Albums, response.Rows[1].Value);

            response.Rows[2].Id.Should().Be("3");
            response.Rows[2].Key.Should().Be("Fake artist 3");
            response.Rows[2].Value.Length.Should().Be(3);
            CustomAsserts.AreValueEqual(TestData.Artists.Artist3.Albums, response.Rows[2].Value);

            response.Rows[3].Id.Should().Be("4");
            response.Rows[3].Key.Should().Be("Fake artist 4");
            response.Rows[3].Value.Length.Should().Be(4);
            CustomAsserts.AreValueEqual(TestData.Artists.Artist4.Albums, response.Rows[3].Value);
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