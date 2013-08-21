namespace MyCouch.IntegrationTests.ClientTests
{
    public abstract class ClientTestsOf<T> : IntegrationTests<IClient, T> where T : class
    {
        protected ClientTestsOf() 
            : base(IntegrationTestsRuntime.CreateClient())
        {
        }
    }
}