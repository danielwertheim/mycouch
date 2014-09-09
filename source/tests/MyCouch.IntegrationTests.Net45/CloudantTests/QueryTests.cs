using MyCouch.Cloudant;
using MyCouch.Cloudant.Requests;
using MyCouch.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;

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
            var indexRequest = new IndexRequest();
            indexRequest.Configure(q => q.DesignDocument("MyDoc")
                .Name("MyName")
                .Fields(new IndexField("diet"))
                );

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Creating_an_index_with_same_name_as_existing_should_be_reported()
        {
            var indexRequest = new IndexRequest();
            indexRequest.Configure(q => q.DesignDocument("MyDoc")
                .Name("MyName")
                .Fields(new IndexField("diet"))
                );
            SUT.PostAsync(indexRequest).Wait();

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("exists");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Can_create_an_index_without_specifying_a_designdoc_and_name()
        {
            var indexRequest = new IndexRequest();
            indexRequest.Configure(q => q.Fields(new IndexField("diet")));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Can_specify_a_sort_order_for_an_index_field()
        {
            var indexRequest = new IndexRequest();
            indexRequest.Configure(q => q.Fields(new IndexField("diet", SortDirection.Desc)));

            var response = SUT.PostAsync(indexRequest).Result;

            response.IsSuccess.Should().Be(true);
            response.Result.Should().Be("created");
        }
    }
}
