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
            //TODO: Use batch delete instead
            var query = new SystemViewQuery("_all_docs");
            var response = Client.Views.RunQuery<dynamic>(query);

            if (!response.IsEmpty)
                foreach (var row in response.Rows)
                    Client.Documents.Delete(row.Id, row.Value.rev.ToString());
        }
    }
}