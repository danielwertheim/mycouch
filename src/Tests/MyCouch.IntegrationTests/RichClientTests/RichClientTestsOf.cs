using MyCouch.Rich;

namespace MyCouch.IntegrationTests.RichClientTests
{
    public abstract class RichClientTestsOf<T> : IntegrationTests<IRichClient, T> where T : class
    {
        protected RichClientTestsOf() 
            : base(IntegrationTestsRuntime.CreateRichClient())
        {
        }
    }
}