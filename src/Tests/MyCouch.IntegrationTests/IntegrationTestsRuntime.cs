using MyCouch.Querying;

namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        internal static IClient Client { get; private set; }

        internal static void Init()
        {
            Client = TestClientFactory.CreateDefault();
            //Client.Databases.Put(TestConstants.TestDbName);
            ClearAllDocuments();
        }

        internal static void Close()
        {
            Client.Dispose();
            Client = null;
        }

        internal static void ClearAllDocuments()
        {
            var query = new SystemViewQuery("_all_docs");
            var response = Client.Views.RunQuery<dynamic>(query);

            if (!response.IsEmpty)
            {
                var bulkCmd = new BulkCommand();
                foreach (var row in response.Rows)
                    bulkCmd.Delete(row.Id, row.Value.rev.ToString());
                Client.Documents.Bulk(bulkCmd);
            }
        }
    }
}