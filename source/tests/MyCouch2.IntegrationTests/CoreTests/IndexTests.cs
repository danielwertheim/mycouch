using FluentAssertions;
using MyCouch.Requests;
using System.Linq;
using System.Net;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    [Trait("Category", "IntegrationTests.CoreTests")]
    public class IndexTests : IntegrationTestsOf<IQueries>
    {
        public IndexTests()
        {
            SUT = DbClient.Queries;
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Can_create_an_index_with_explicit_designdoc_and_name()
        {
            var indexRequest = new PostIndexRequest().Configure(q => q
                .DesignDocument("MyDoc")
                .Name("MyName")
                .Fields(new SortableField("diet")));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Creating_an_index_with_same_name_as_existing_should_be_reported()
        {
            var indexRequest = new PostIndexRequest().Configure(q => q
                .DesignDocument("MyDoc")
                .Name("MyName")
                .Fields(new SortableField("diet")));

            SUT.PostAsync(indexRequest).Wait();

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("exists");
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Can_create_an_index_without_specifying_a_designdoc_and_name()
        {
            var indexRequest = new PostIndexRequest().Configure(q => q
                .Fields(new SortableField("diet")));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Can_specify_a_sort_order_for_an_index_field()
        {
            var indexRequest = new PostIndexRequest().Configure(q => q
                .Fields(new SortableField("diet")));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Can_delete_a_pre_existing_index()
        {
            const string dDocName = "MyDoc";
            const string indexName = "MyName";
            var indexRequest = new PostIndexRequest().Configure(q => q
                .DesignDocument(dDocName)
                .Name(indexName)
                .Fields(new SortableField("diet")));

            SUT.PostAsync(indexRequest).Wait();

            var response = SUT.DeleteAsync(new DeleteIndexRequest(dDocName, indexName)).Result;
            response.IsSuccess.Should().Be(true);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Trying_to_delete_a_non_existing_index_should_report_error()
        {
            var response = SUT.DeleteAsync(new DeleteIndexRequest("junk", "junk")).Result;

            response.IsSuccess.Should().Be(false);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Reason.Should().Be("missing");
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Getall_should_return_all_indexes()
        {
            var response = SUT.GetAllAsync().Result;

            response.IsSuccess.Should().BeTrue();
            response.IsEmpty.Should().BeFalse();
            response.IndexCount.Should().Be(1);
            var primaryIndex = response.Indexes.First();

            primaryIndex.Name.Should().Be("_all_docs");
            primaryIndex.Def.Fields.Count().Should().Be(1);
            var field = primaryIndex.Def.Fields.First();
            field.Name.Should().Be("_id");
            field.SortDirection.Should().Be(SortDirection.Asc);
        }
    }
}
