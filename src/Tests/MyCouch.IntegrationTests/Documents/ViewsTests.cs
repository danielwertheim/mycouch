using NUnit.Framework;

namespace MyCouch.IntegrationTests.Documents
{
    [TestFixture]
    public class ViewsTests : IntegrationTestsOf<IViews>
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = Client.Views;
        }

        protected override void OnTestFinalize()
        {
            base.OnTestFinalize();

            IntegrationTestsRuntime.ClearAllDocuments();
        }
    }
}