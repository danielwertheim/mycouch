using System.Linq;
using FluentAssertions;
using MyCouch.Cloudant;
using MyCouch.Cloudant.Requests;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CloudantTests
{
    public class SearchTests : CloudantTestsOf<ISearches>, IPreserveStatePerFixture, IUseFixture<SearchFixture>
    {
        protected Animal[] Animals { get; set; }

        protected override void OnTestInit()
        {
            SUT = Client.Searches;
        }

        public void SetFixture(SearchFixture data)
        {
            Animals = data.Animals;
        }

        [Fact]
        public void Can_search_on_default_index_using_simple_expression()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("kookaburra"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.Bookmark.Should().NotBeNullOrEmpty();
            response.Rows[0].Id.Should().Be("kookaburra");
            response.Rows[0].Order.Should().BeEquivalentTo(new[] {1.4054651260375977, 0});
            response.Rows[0].Fields.Count.Should().Be(4);
            response.Rows[0].Fields["diet"].Should().Be("carnivore");
            response.Rows[0].Fields["minLength"].Should().Be(0.28);
            response.Rows[0].Fields["class"].Should().Be("bird");
            response.Rows[0].Fields["latinName"].Should().Be("Dacelo novaeguineae");
            response.Rows[0].IncludedDoc.Should().BeNull();
        }

        [Fact]
        public void Can_search_on_more_complex_expressions()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("diet:carnivore AND minLength:[1 TO 3]"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.Bookmark.Should().NotBeNullOrEmpty();
            response.Rows[0].Id.Should().Be("panda");
            response.Rows[0].Order.Should().BeEquivalentTo(new[] { 1.4142135381698608, 1 });
            response.Rows[0].Fields.Count.Should().Be(3);
            response.Rows[0].Fields["diet"].Should().Be("carnivore");
            response.Rows[0].Fields["minLength"].Should().Be(1.2);
            response.Rows[0].Fields["class"].Should().Be("mammal");
            response.Rows[0].IncludedDoc.Should().BeNull();
        }

        [Fact]
        public void Can_sort()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("diet:carnivore")
                .Sort("-minLength<number>"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 2);
            response.Bookmark.Should().NotBeNullOrEmpty();
            response.Rows[0].Id.Should().Be("panda");
            response.Rows[0].Order.Should().BeEquivalentTo(new[] { 1.2, 1 });
            response.Rows[0].Fields["diet"].Should().Be("carnivore");
            response.Rows[0].Fields["minLength"].Should().Be(1.2);

            response.Rows[1].Id.Should().Be("kookaburra");
            response.Rows[1].Order.Should().BeEquivalentTo(new[] { 0.28, 0 });
            response.Rows[1].Fields["diet"].Should().Be("carnivore");
            response.Rows[1].Fields["minLength"].Should().Be(0.28);
        }

        [Fact]
        public void Can_include_docs()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("kookaburra")
                .IncludeDocs(true));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);

            var doc = Client.Documents.GetAsync(response.Rows[0].Id).Result;
            response.Rows[0].IncludedDoc.Should().Be(doc.Content);
        }

        [Fact]
        public void Can_include_docs_to_specific_entity()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("kookaburra")
                .IncludeDocs(true));

            var response = SUT.SearchAsync<Animal>(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);

            var orgDoc = Animals.Single(a => a.AnimalId == response.Rows[0].Id);
            var returnedDoc = response.Rows[0].IncludedDoc;

            CustomAsserts.AreValueEqual(orgDoc, returnedDoc);
        }

        [Fact]
        public void Can_limit()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("class:mammal")
                .Limit(1));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.TotalRows.Should().Be(8);
        }

        [Fact]
        public void Can_navigate_using_bookmarks()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("class:mammal")
                .Limit(1));

            var response1 = SUT.SearchAsync(searchRequest).Result;

            response1.Should().BeSuccessfulGet(numOfRows: 1);
            response1.TotalRows.Should().Be(8);
            response1.Rows[0].Id.Should().Be("panda");

            searchRequest.Configure(q => q
                .Bookmark(response1.Bookmark));

            var response2 = SUT.SearchAsync(searchRequest).Result;
            response2.Should().BeSuccessfulGet(numOfRows: 1);
            response2.TotalRows.Should().Be(8);
            response2.Rows[0].Id.Should().Be("aardvark");
        }
    }
}