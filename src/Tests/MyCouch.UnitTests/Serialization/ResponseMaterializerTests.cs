using System.Collections.Generic;
using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Responses;
using MyCouch.Serialization;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class DefaultResponseMaterializerWithSimpleContractResolverTests : ResponseMaterializerTests
    {
        public DefaultResponseMaterializerWithSimpleContractResolverTests()
        {
            SUT = new DefaultResponseMaterializer(new SerializationConfiguration(new SerializationContractResolver()));
        }
    }

    public class DefaultResponseMaterializerWithEntityContractResolverUsingLambdasTests : ResponseMaterializerTests
    {
        public DefaultResponseMaterializerWithEntityContractResolverUsingLambdasTests()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
            SUT = new DefaultResponseMaterializer(new SerializationConfiguration(new EntitySerializationContractResolver(entityReflector)));
        }
    }

#if !NETFX_CORE
    public class DefaultResponseMaterializerWithEntityContractResolverUsingIlTests : ResponseMaterializerTests
    {
        public DefaultResponseMaterializerWithEntityContractResolverUsingIlTests()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());
            SUT = new DefaultResponseMaterializer(new SerializationConfiguration(new EntitySerializationContractResolver(entityReflector)));
        }
    }
#endif

    public abstract class ResponseMaterializerTests : UnitTestsOf<DefaultResponseMaterializer>
    {
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
    }
}
