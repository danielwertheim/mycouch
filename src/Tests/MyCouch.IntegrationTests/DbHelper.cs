using System.Collections.Generic;

namespace MyCouch.IntegrationTests
{
    internal static class DbHelper
    {
        internal static void ClearAllDocuments()
        {
            using (var client = TestClientFactory.CreateDefault())
            {
                var query = client.Views.CreateSystemQuery("_all_docs");
                var response = client.Views.RunQuery<dynamic>(query);
                foreach (var row in response.Rows)
                {
                    client.Documents.Delete(row.Id, row.Value.rev.ToString());
                }
            }
        }
    }
}