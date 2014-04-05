using System.Linq;
using Xunit;

namespace MyCouch.IntegrationTests
{
    public class MyFactAttribute : FactAttribute
    {
        public MyFactAttribute(params string[] scenarios)
        {
            if(!scenarios.Any() || IntegrationTestsRuntime.Environment.SupportsEverything)
                return;

            if (!scenarios.All(r => IntegrationTestsRuntime.Environment.Supports.Any(s => r == s)))
                Skip = string.Format("TestEnvironment does not support ALL test scenario(s): '{0}'.", string.Join("|", scenarios));
        }
    }
}