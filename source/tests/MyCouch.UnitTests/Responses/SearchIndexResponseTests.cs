using FluentAssertions;
using MyCouch.Responses;
using Xunit;

namespace MyCouch.UnitTests.Responses
{
    public class SearchIndexResponseTests : UnitTestsOf<SearchIndexResponse>
    {
        [Fact]
        public void When_order_is_array_of_doubles_GetOrderAsDoubles_returns_doubles()
        {
            SUT = new SearchIndexResponse
            {
                Rows = new[]
                {
                    new SearchIndexResponse<string>.Row
                    {
                        Order = new object[] { 1.2, 1.3 }
                    }
                }
            };

            SUT.Rows[0].GetOrderAsDoubles().Should().BeEquivalentTo(1.2, 1.3);
        }
    }
}