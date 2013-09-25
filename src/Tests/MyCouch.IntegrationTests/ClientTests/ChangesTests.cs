using System;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class ChangesTests : ClientTestsOf<IChanges>
    {
        protected override void OnTestInit()
        {
            SUT = Client.Changes;
        }

        [Fact]
        public void When_getting_continuous_changes_It_will_return_changes_as_they_occur()
        {
            throw new NotImplementedException();
            //var response = SUT.GetContinuouslyAsync().Result;
        }
    }
}