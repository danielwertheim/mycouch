using MyCouch.Requests;
using MyCouch.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;
using System.Linq;
using MyCouch.Testing.Model;
using Newtonsoft.Json.Linq;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class QueryTests : IntegrationTestsOf<IQueries>,
        IPreserveStatePerFixture,
        IClassFixture<QueriesFixture>
    {
        public QueryTests(QueriesFixture data)
        {
            data.Init(Environment);
            SUT = DbClient.Queries;
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Query_with_simple_implicit_selector_should_return_matching_docs()
        {
            const string title = "Html5 blog";
            const int age = 21;
            var request = new FindRequest().Configure(q =>
                q.SelectorExpression("{{\"title\": \"{0}\", \"author.age\": {1}}}", title, age));

            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(1);
            var doc = response.Docs.First();
            doc.Title.Should().Be(title);
            doc.Author.Age.Should().Be(age);
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Query_with_simple_conditional_operator_should_return_matching_docs()
        {
            const int age = 21;
            var selector = new JObject
            {
                {"author.age", new JObject {{"$gt", age}}}
            };
            var request = new FindRequest().Configure(q => q
                .SelectorExpression(selector.ToString())
                .Fields("title"));

            var response = SUT.FindAsync(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(2);
            response.Docs.Should().Contain(t => t.Contains("Couch blog"));
            response.Docs.Should().Contain(t => t.Contains("Json blog"));
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Query_with_combintation_operators_should_return_matching_docs()
        {
            const string e = "{\"$and\":[{\"author.age\":{\"$gt\":21}},{\"yearsActive\":{\"$lt\":5}}]}";

            var request = new FindRequest().Configure(q => q
                .SelectorExpression(e)
                .Fields("title"));

            var response = SUT.FindAsync(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(1);
            response.Docs.Should().Contain(d => d.Contains("Couch blog"));
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Query_with_specified_sort_fields_should_return_sorted_results()
        {
            var sortedAges = new[] { 43, 32 };

            var request = new FindRequest().Configure(q => q
                .SelectorExpression("{\"author.age\": {\"$gt\": 21}}")
                .Fields("author.age")
                .Sort(new SortableField("author.age", SortDirection.Desc)));

            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().BeTrue();
            response.DocCount.Should().Be(2);
            response.Docs.Select(b => b.Author.Age).SequenceEqual(sortedAges.Select(y => y)).Should().BeTrue();
        }

        [MyFact(TestScenarios.QueriesContext)]
        public void Query_can_skip_and_limit_results()
        {
            var titles = new[] { "Couch blog" };
            var request = new FindRequest().Configure(q => q
                .SelectorExpression("{\"author.age\": {\"$gt\": 0}}")
                .Fields("title")
                .Skip(1)
                .Limit(1));

            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().BeTrue();
            response.DocCount.Should().Be(1);
            response.Docs.Select(b => b.Title).SequenceEqual(titles.Select(y => y)).Should().BeTrue();
        }
    }
}
