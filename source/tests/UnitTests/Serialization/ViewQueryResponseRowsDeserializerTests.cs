﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using MyCouch.Responses;
using MyCouch.Serialization;
using MyCouch.Serialization.Meta;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace UnitTests.Serialization
{
    public class ViewQueryResponseRowsDeserializerWithEntityContractResolverUsingLambdasTests : ViewQueryResponseRowsDeserializerTests
    {
        public ViewQueryResponseRowsDeserializerWithEntityContractResolverUsingLambdasTests()
            : base(CreateEntityReflector()) { }

        private static IEntityReflector CreateEntityReflector()
        {
            return new EntityReflector(new LambdaDynamicPropertyFactory());
        }
    }

    public class ViewQueryResponseRowsDeserializerWithEntityContractResolverUsingIlTests : ViewQueryResponseRowsDeserializerTests
    {
        public ViewQueryResponseRowsDeserializerWithEntityContractResolverUsingIlTests()
            : base(CreateEntityReflector()) { }

        private static IEntityReflector CreateEntityReflector()
        {
            return new EntityReflector(new IlDynamicPropertyFactory());
        }
    }

    public abstract class ViewQueryResponseRowsDeserializerTests : UnitTestsOf<DefaultSerializer>
    {
        protected ViewQueryResponseRowsDeserializerTests(IEntityReflector entityReflector)
        {
            SUT = new DefaultSerializer(new SerializationConfiguration(new SerializationContractResolver()), new DocumentSerializationMetaProvider(), entityReflector);
        }

        [Fact]
        public void It_can_populate_an_all_docs_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestdata.ViewQueryAllDocRows);

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
            var rows = Deserialize<dynamic>(JsonTestdata.ViewQueryAllDocRows);

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
            var rows = Deserialize<IDictionary<string, object>>(JsonTestdata.ViewQueryAllDocRows);

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
        public void It_can_populate_single_value_keys_in_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestdata.ViewQuerySingleValueKeysRows);

            rows.Length.Should().Be(4);
            rows[0].Id.Should().Be("integer:1");
            rows[0].Key.Should().Be((long)1);

            rows[1].Id.Should().Be("float:1");
            rows[1].Key.Should().Be(3.14);

            rows[2].Id.Should().Be("datetime:1");
            rows[2].Key.Should().Be(new DateTime(2013, 09, 22, 22, 36, 00));

            rows[3].Id.Should().Be("string:1");
            rows[3].Key.Should().Be("test1");
        }

        [Fact]
        public void It_can_populate_complex_keys_in_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestdata.ViewQueryComplexKeysRows);

            rows.Length.Should().Be(2);
            rows[0].Id.Should().Be("complex:1");
            ((object[])rows[0].Key).Should().BeEquivalentTo(new object[] { "test1", (long)1, 3.14, new DateTime(2013, 09, 22, 22, 36, 00) });

            rows[1].Id.Should().Be("complex:2");
            ((object[])rows[1].Key).Should().BeEquivalentTo(new object[] { "test2", (long)2, 3.15, new DateTime(2013, 09, 22, 22, 37, 00) });
        }

        [Fact]
        public void It_can_populate_an_albums_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestdata.ViewQueryAlbumRows);

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
            var rows = Deserialize<string[]>(JsonTestdata.ViewQueryAlbumRows);

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
            var rows = Deserialize<Album[]>(JsonTestdata.ViewQueryAlbumRows);

            rows.Length.Should().Be(4);
            rows[0].Id.Should().Be("1");
            rows[0].Key.Should().Be("Fake artist 1");
            rows[0].Value.Length.Should().Be(1);
            rows[0].Value.Should().BeEquivalentTo(ClientTestData.Artists.Artist1.Albums);

            rows[1].Id.Should().Be("2");
            rows[1].Key.Should().Be("Fake artist 2");
            rows[1].Value.Length.Should().Be(2);
            rows[1].Value.Should().BeEquivalentTo(ClientTestData.Artists.Artist2.Albums);

            rows[2].Id.Should().Be("3");
            rows[2].Key.Should().Be("Fake artist 3");
            rows[2].Value.Length.Should().Be(3);
            rows[2].Value.Should().BeEquivalentTo(ClientTestData.Artists.Artist3.Albums);

            rows[3].Id.Should().Be("4");
            rows[3].Key.Should().Be("Fake artist 4");
            rows[3].Value.Length.Should().Be(4);
            rows[3].Value.Should().BeEquivalentTo(ClientTestData.Artists.Artist4.Albums);
        }

        [Fact]
        public void It_can_populate_complex_keys_that_contains_arrays_in_view_query_response_of_string()
        {
            var rows = Deserialize<string>(JsonTestdata.ViewQueryComplexKeyWithArray);

            rows.Length.Should().Be(1);
            var key = (object[])rows[0].Key;
            key[0].As<object[]>().Select(i => i as string).Should().BeEquivalentTo(new [] {"a", "b"});
            key[1].As<long>().Should().Be(123234);
        }

        private ViewQueryResponse<T>.Row[] Deserialize<T>(string jsonRows) where T : class
        {
            using (var content = jsonRows.AsStream())
            {
                return SUT.Deserialize<ViewQueryResponse<T>.Row[]>(content);
            }
        }
    }
}
