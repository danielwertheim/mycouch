using MyCouch.Cloudant;
using MyCouch.Cloudant.Requests;
using MyCouch.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;
using System.Net;
using System.Linq;
using MyCouch.Testing.Model;

namespace MyCouch.IntegrationTests.CloudantTests
{
    public class QueryTests : IntegrationTestsOf<IQueries>,
        IPreserveStatePerFixture,
        IUseFixture<QueriesFixture>
    {
        public QueryTests()
        {
            SUT = CloudantDbClient.Queries;
        }
        public void SetFixture(QueriesFixture data)
        {
            data.Init(Environment);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Can_create_an_index_with_explicit_designdoc_and_name()
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.DesignDocument("MyDoc")
                .Name("MyName")
                .Fields(new SortableField("diet"))
                );

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Creating_an_index_with_same_name_as_existing_should_be_reported()
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.DesignDocument("MyDoc")
                .Name("MyName")
                .Fields(new SortableField("diet"))
                );
            SUT.PostAsync(indexRequest).Wait();

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("exists");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Can_create_an_index_without_specifying_a_designdoc_and_name()
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.Fields(new SortableField("diet")));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Can_specify_a_sort_order_for_an_index_field()
        {
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.Fields(new SortableField("diet", SortDirection.Desc)));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Can_delete_a_pre_existing_index()
        {
            var dDocName = "MyDoc";
            var indexName = "MyName";
            var indexRequest = new PostIndexRequest();
            indexRequest.Configure(q => q.DesignDocument(dDocName)
                .Name(indexName)
                .Fields(new SortableField("diet"))
                );
            SUT.PostAsync(indexRequest).Wait();

            var response = SUT.DeleteAsync(new DeleteIndexRequest(dDocName, indexName)).Result;
            response.IsSuccess.Should().Be(true);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Trying_to_delete_a_non_existing_index_should_report_error()
        {
            var response = SUT.DeleteAsync(new DeleteIndexRequest("junk", "junk")).Result;
            response.IsSuccess.Should().Be(false);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Reason.Should().Be("missing");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Getall_should_return_all_indexes()
        {
            var response = SUT.GetAllAsync().Result;

            response.IsSuccess.Should().Be(true);
            response.IsEmpty.Should().Be(false);
            response.IndexCount.Should().Be(1);
            var primaryIndex = response.Indexes.First();

            primaryIndex.Name.Should().Be("_all_docs");
            primaryIndex.Def.Fields.Count().Should().Be(1);
            var field = primaryIndex.Def.Fields.First();
            field.Name.Should().Be("_id");
            field.SortDirection.Should().Be(SortDirection.Asc);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Query_with_simple_selectors_should_return_matching_docs()
        {
            const string searchTitle = "Html5 blog";
            var request = new FindRequest();
            request.Configure(q => q.Selector(new { title = searchTitle }));

            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(1);
            var doc = response.Docs.First();
            doc.Title.Should().Be(searchTitle);
        }
    }
}
