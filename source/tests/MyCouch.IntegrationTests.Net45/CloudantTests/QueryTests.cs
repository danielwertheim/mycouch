using MyCouch.Cloudant;
using MyCouch.Cloudant.Requests;
using MyCouch.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;
using System.Net;
using System.Linq;
using MyCouch.Testing.Model;
using Newtonsoft.Json.Linq;

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
        public void Query_with_simple_implicit_selector_should_return_matching_docs()
        {
            const string title = "Html5 blog";
            const int age = 21;
            var request = new FindRequest();

            var formatString = "{{\"title\": \"{0}\", \"author.age\": {1}}}";
            request.Configure(q => q.SelectorExpression(string.Format(formatString, title, age)));
            
            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(1);
            var doc = response.Docs.First();
            doc.Title.Should().Be(title);
            doc.Author.Age.Should().Be(age);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Query_with_simple_conditional_operator_should_return_matching_docs()
        {
            const int age = 21;
            JObject condition = new JObject();
            condition.Add("$gt", age);
            JObject selector = new JObject();
            selector.Add("author.age", condition);
            var request = new FindRequest();
            request.Configure(q => q.SelectorExpression(selector.ToString())
                .Fields("title")
                );

            var response = SUT.FindAsync(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(2);
            response.Docs.Select(t => t.Contains("Couch blog"));
            response.Docs.Select(t => t.Contains("Json blog"));
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Query_with_combintation_operators_should_return_matching_docs()
        {
            const int age = 21;
            const int yearsActive = 5;
            JObject gt = new JObject();
            gt.Add("$gt", age);
            JObject lt = new JObject();
            lt.Add("$lt", yearsActive);
            JObject ageCondition = new JObject();
            ageCondition.Add("author.age", gt);
            JObject yrsAtiveCondition = new JObject();
            yrsAtiveCondition.Add("yearsActive", lt);
            JObject selector = new JObject();
            selector.Add("$and", new JArray(ageCondition, yrsAtiveCondition));

            var request = new FindRequest();
            request.Configure(q => q.SelectorExpression(selector.ToString())
                .Fields("title")
                );

            var response = SUT.FindAsync(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(1);
            response.Docs.Select(t => t.Contains("Couch blog"));
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Query_with_specified_sort_fields_should_return_sorted_results()
        {
            var sortedAges = new[] { 43, 32 };
            const int age = 21;
            JObject condition = new JObject();
            condition.Add("$gt", age);
            JObject selector = new JObject();
            selector.Add("author.age", condition);
            var request = new FindRequest();
            request.Configure(q => q.SelectorExpression(selector.ToString())
                .Fields("author.age")
                .Sort(new SortableField("author.age", SortDirection.Desc))
                );

            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(2);
            response.Docs.Select(b => b.Author.Age).SequenceEqual(sortedAges.Select(y => y)).Should().Be(true);
        }

        [MyFact(TestScenarios.Cloudant, TestScenarios.QueriesContext)]
        public void Query_can_skip_and_limit_results()
        {
            var titles = new[] { "Couch blog" };
            const int age = 0;
            JObject condition = new JObject();
            condition.Add("$gt", age);
            JObject selector = new JObject();
            selector.Add("author.age", condition);
            var request = new FindRequest();
            request.Configure(q => q.SelectorExpression(selector.ToString())
                .Fields("title")
                .Skip(1)
                .Limit(1)
                );

            var response = SUT.FindAsync<Blog>(request).Result;

            response.IsSuccess.Should().Be(true);
            response.DocCount.Should().Be(1);
            response.Docs.Select(b => b.Title).SequenceEqual(titles.Select(y => y)).Should().Be(true);
        }
    }
}
