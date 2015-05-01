using FluentAssertions;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class ClientTests : IntegrationTestsOf<IMyCouchClient>
    {
        public ClientTests()
        {
            SUT = DbClient;
        }

        [MyFact(TestScenarios.Client)]
        public async void Should_invoke_BeforeSend_if_hooked_in()
        {
            var wasCalled = false;

            SUT.Connection.BeforeSend = _ => wasCalled = true;

            await SUT.Database.HeadAsync();

            wasCalled.Should().BeTrue();
        }

        [MyFact(TestScenarios.Client)]
        public async void Should_invoke_AfterSend_if_hooked_in()
        {
            var wasCalled = false;

            SUT.Connection.AfterSend = _ =>
            {
                wasCalled = true;
            };

            await SUT.Database.HeadAsync();

            wasCalled.Should().BeTrue();
        }
    }
}