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
    [Trait("Category", "IntegrationTests.CloudantTests")]
    public class SearchTests : IntegrationTestsOf<ISearches>,
        IPreserveStatePerFixture,
        IClassFixture<SearchFixture>
    {
        protected Animal[] Animals { get; set; }

        public SearchTests(SearchFixture data)
        {
            Animals = data.Init(Environment);
            SUT = CloudantDbClient.Searches;
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_search_on_default_index_using_simple_expression()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("kookaburra"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.Bookmark.Should().NotBeNullOrEmpty();
            response.Rows[0].Id.Should().Be("kookaburra");
            response.Rows[0].Order[0].Should().Be(1.4054651260375977);
            response.Rows[0].Order[1].Should().Be((long)0);
            response.Rows[0].Fields.Count.Should().Be(5);
            response.Rows[0].Fields["diet"].Should().Be("carnivore");
            response.Rows[0].Fields["minLength"].Should().Be(0.28);
            response.Rows[0].Fields["class"].Should().Be("bird");
            response.Rows[0].Fields["latinName"].Should().Be("Dacelo novaeguineae");
            response.Rows[0].IncludedDoc.Should().BeNull();
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_search_on_more_complex_expressions()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("diet:carnivore AND minLength:[1 TO 3]"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.Bookmark.Should().NotBeNullOrEmpty();
            response.Rows[0].Id.Should().Be("panda");
            response.Rows[0].Order[0].Should().Be(1.4142135381698608);
            response.Rows[0].Order[1].Should().Be((long)1);
            response.Rows[0].Fields.Count.Should().Be(4);
            response.Rows[0].Fields["diet"].Should().Be("carnivore");
            response.Rows[0].Fields["minLength"].Should().Be(1.2);
            response.Rows[0].Fields["class"].Should().Be("mammal");
            response.Rows[0].IncludedDoc.Should().BeNull();
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_sort()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("diet:carnivore")
                .Sort("-minLength<number>"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 2);
            response.Bookmark.Should().NotBeNullOrEmpty();
            response.Rows[0].Id.Should().Be("panda");
            response.Rows[0].Order[0].Should().Be(1.2);
            response.Rows[0].Order[1].Should().Be((long)1);
            response.Rows[0].Fields["diet"].Should().Be("carnivore");
            response.Rows[0].Fields["minLength"].Should().Be(1.2);

            response.Rows[1].Id.Should().Be("kookaburra");
            response.Rows[1].Order[0].Should().Be(0.28);
            response.Rows[1].Order[1].Should().Be((long)0);
            response.Rows[1].Fields["diet"].Should().Be("carnivore");
            response.Rows[1].Fields["minLength"].Should().Be(0.28);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_include_docs()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("kookaburra")
                .IncludeDocs(true));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);

            var doc = CloudantDbClient.Documents.GetAsync(response.Rows[0].Id).Result;
            response.Rows[0].IncludedDoc.Should().Be(doc.Content);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_include_docs_to_specific_entity()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("kookaburra")
                .IncludeDocs(true));

            var response = SUT.SearchAsync<Animal>(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);

            var orgDoc = Animals.Single(a => a.AnimalId == response.Rows[0].Id);
            var returnedDoc = response.Rows[0].IncludedDoc;

            returnedDoc.ShouldBeEquivalentTo(orgDoc);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_limit()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("class:mammal")
                .Limit(1));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.TotalRows.Should().Be(8);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
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

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_report_counts_by_multiple_fields()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("minLength:[1 TO 3]")
                .Counts("diet", "class"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 4);
            response.Counts.Should().NotBeNullOrWhiteSpace();
            var counts = CloudantDbClient.Serializer.Deserialize<dynamic>(response.Counts);
            ((double)counts.@class.mammal).Should().Be(4.0);
            ((double)counts.diet.carnivore).Should().Be(1.0);
            ((double)counts.diet.herbivore).Should().Be(2.0);
            ((double)counts.diet.omnivore).Should().Be(1.0);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_report_ranges_by_multiple_fields()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("minLength:[1 TO 3]")
                .Ranges(
                    new
                    {
                        minLength = new { minLow = "[0 TO 100]", minHigh = "{101 TO Infinity}" },
                        maxLength = new { maxLow = "[0 TO 100]", maxHigh = "{101 TO Infinity}" }
                    }
                ));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 4);
            response.Ranges.Should().NotBeNullOrWhiteSpace();
            var ranges = CloudantDbClient.Serializer.Deserialize<dynamic>(response.Ranges);
            ((double)ranges.minLength.minLow).Should().Be(4.0);
            ((double)ranges.minLength.minHigh).Should().Be(0.0);
            ((double)ranges.maxLength.maxLow).Should().Be(4.0);
            ((double)ranges.maxLength.maxHigh).Should().Be(0.0);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.SearchesContext)]
        public void Can_drilldown_searches_by_field_value()
        {
            var searchRequest = new SearchIndexRequest(CloudantTestData.Views.Views101AnimalsSearchIndexId).Configure(q => q
                .Expression("class:(bird OR mammal)")
                .Counts("diet")
                .DrillDown("class", "bird"));

            var response = SUT.SearchAsync(searchRequest).Result;

            response.Should().BeSuccessfulGet(numOfRows: 2);
            response.Rows.Should().NotContain(r => (string)r.Fields["class"] != "bird");
            response.Counts.Should().NotBeNullOrWhiteSpace();
            var counts = CloudantDbClient.Serializer.Deserialize<dynamic>(response.Counts);
            ((double)counts.diet.carnivore).Should().Be(1.0);
            ((double)counts.diet.omnivore).Should().Be(1.0);
        }
    }
}