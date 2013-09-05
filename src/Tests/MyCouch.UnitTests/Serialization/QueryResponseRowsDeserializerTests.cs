using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Responses;
using MyCouch.Serialization;
using MyCouch.Serialization.Readers;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Newtonsoft.Json;
using Xunit;

namespace MyCouch.UnitTests.Serialization
{
    public class QueryResponseRowsDeserializerWithSimpleContractResolverTests : QueryResponseRowsDeserializerTests
    {
        public QueryResponseRowsDeserializerWithSimpleContractResolverTests() : base(CreateSerializationConfiguration()) { }

        private static SerializationConfiguration CreateSerializationConfiguration()
        {
            return new SerializationConfiguration(new SerializationContractResolver());
        }
    }

    public class QueryResponseMaterializerWithEntityContractResolverUsingLambdasTests : QueryResponseRowsDeserializerTests
    {
        public QueryResponseMaterializerWithEntityContractResolverUsingLambdasTests() : base(CreateSerializationConfiguration()) { }

        private static SerializationConfiguration CreateSerializationConfiguration()
        {
            var entityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());

            return new SerializationConfiguration(new EntityContractResolver(entityReflector));
        }
    }

#if !NETFX_CORE
    public class QueryResponseRowsDeserializerWithEntityContractResolverUsingIlTests : QueryResponseRowsDeserializerTests
    {
        public QueryResponseRowsDeserializerWithEntityContractResolverUsingIlTests() : base(CreateSerializationConfiguration()) { }

        private static SerializationConfiguration CreateSerializationConfiguration()
        {
            var entityReflector = new EntityReflector(new IlDynamicPropertyFactory());

            return new SerializationConfiguration(new EntityContractResolver(entityReflector));
        }
    }
#endif

    public abstract class QueryResponseRowsDeserializerTests : UnitTestsOf<QueryResponseRowsDeserializer>
    {
        protected readonly SerializationConfiguration SerializationConfiguration;

        protected QueryResponseRowsDeserializerTests(SerializationConfiguration serializationConfiguration)
        {
            SerializationConfiguration = serializationConfiguration;
            SUT = new QueryResponseRowsDeserializer(SerializationConfiguration);
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestData.ViewQueryAllDocRows);

            rows.Length.Should().Be(2);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("1");
            rows[0].Value.Should().Be("{\"rev\":\"43-4886b6a3da60a647adea18b1c6c81cd5\"}");

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("2");
            rows[1].Value.Should().Be("{\"rev\":\"42-e7620ba0ea71c48f6a11bacee4999d79\"}");
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_dynamic()
        {
            var rows = Deserialize<dynamic>(JsonTestData.ViewQueryAllDocRows);

            rows.Length.Should().Be(2);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("1");
            string rev1 = rows[0].Value.rev;
            rev1.Should().Be("43-4886b6a3da60a647adea18b1c6c81cd5");

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("2");
            string rev2 = rows[1].Value.rev;
            rev2.Should().Be("42-e7620ba0ea71c48f6a11bacee4999d79");
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_dictionary()
        {
            var rows = Deserialize<IDictionary<string, object>>(JsonTestData.ViewQueryAllDocRows);

            rows.Length.Should().Be(2);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("1");
            string rev1 = rows[0].Value["rev"].ToString();
            rev1.Should().Be("43-4886b6a3da60a647adea18b1c6c81cd5");

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("2");
            string rev2 = rows[1].Value["rev"].ToString();
            rev2.Should().Be("42-e7620ba0ea71c48f6a11bacee4999d79");
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestData.ViewQueryAlbumRows);

            rows.Length.Should().Be(4);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("Fake artist 1");
            rows[0].Value.Should().Be("[{\"name\":\"Greatest fakes #1\"}]");

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("Fake artist 2");
            rows[1].Value.Should().Be("[{\"name\":\"Greatest fakes #2.1\"},{\"name\":\"Greatest fakes #2.2\"}]");

            rows[2].Id.Should().Be("3");
            rows[2].Key.Should().Be("Fake artist 3");
            rows[2].Value.Should().Be("[{\"name\":\"Greatest fakes #3.1\"},{\"name\":\"Greatest fakes #3.2\"},{\"name\":\"Greatest fakes #3.3\"}]");

            rows[3].Id.Should().Be("4");
            rows[3].Key.Should().Be("Fake artist 4");
            rows[3].Value.Should().Be("[{\"name\":\"Greatest fakes #4.1\"},{\"name\":\"Greatest fakes #4.2\"},{\"name\":\"Greatest fakes #4.3\"},{\"name\":\"Greatest fakes #4.4\"}]");
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_strings()
        {
            var rows = Deserialize<string[]>(JsonTestData.ViewQueryAlbumRows);

            rows.Length.Should().Be(4);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("Fake artist 1");
            rows[0].Value.Length.Should().Be(1);
            rows[0].Value[0].Should().Be("{\"name\":\"Greatest fakes #1\"}");

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("Fake artist 2");
            rows[1].Value.Length.Should().Be(2);
            rows[1].Value[0].Should().Be("{\"name\":\"Greatest fakes #2.1\"}");
            rows[1].Value[1].Should().Be("{\"name\":\"Greatest fakes #2.2\"}");

            rows[2].Id.Should().Be("3");
            rows[2].Key.Should().Be("Fake artist 3");
            rows[2].Value.Length.Should().Be(3);
            rows[2].Value[0].Should().Be("{\"name\":\"Greatest fakes #3.1\"}");
            rows[2].Value[1].Should().Be("{\"name\":\"Greatest fakes #3.2\"}");
            rows[2].Value[2].Should().Be("{\"name\":\"Greatest fakes #3.3\"}");

            rows[3].Id.Should().Be("4");
            rows[3].Key.Should().Be("Fake artist 4");
            rows[3].Value.Length.Should().Be(4);
            rows[3].Value[0].Should().Be("{\"name\":\"Greatest fakes #4.1\"}");
            rows[3].Value[1].Should().Be("{\"name\":\"Greatest fakes #4.2\"}");
            rows[3].Value[2].Should().Be("{\"name\":\"Greatest fakes #4.3\"}");
            rows[3].Value[3].Should().Be("{\"name\":\"Greatest fakes #4.4\"}");
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_albums()
        {
            var rows = Deserialize<Album[]>(JsonTestData.ViewQueryAlbumRows);

            rows.Length.Should().Be(4);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("Fake artist 1");
            rows[0].Value.Length.Should().Be(1);
            CustomAsserts.AreValueEqual(ClientTestData.Artists.Artist1.Albums, rows[0].Value);

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("Fake artist 2");
            rows[1].Value.Length.Should().Be(2);
            CustomAsserts.AreValueEqual(ClientTestData.Artists.Artist2.Albums, rows[1].Value);

            rows[2].Id.Should().Be("3");
            rows[2].Key.Should().Be("Fake artist 3");
            rows[2].Value.Length.Should().Be(3);
            CustomAsserts.AreValueEqual(ClientTestData.Artists.Artist3.Albums, rows[2].Value);

            rows[3].Id.Should().Be("4");
            rows[3].Key.Should().Be("Fake artist 4");
            rows[3].Value.Length.Should().Be(4);
            CustomAsserts.AreValueEqual(ClientTestData.Artists.Artist4.Albums, rows[3].Value);
        }

        private QueryResponse<T>.Row[] Deserialize<T>(string jsonRows) where T : class
        {
            using (var sr = new StreamReader(jsonRows.AsStream()))
            using (var jr = SerializationConfiguration.ApplyConfigToReader(new MyCouchJsonReader(sr)))
            {
                if (jr.Read())
                    return SUT.Deserialize<T>(jr).ToArray();
            }

            return null;
        }
    }
}
