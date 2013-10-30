using MyCouch.Requests;

namespace MyCouch.IntegrationTests
{
    internal static class ClientExtensions
    {
        internal static void ClearAllDocuments(this IClient client)
        {
            var query = new QuerySystemViewRequest("_all_docs");
            var response = client.Views.QueryAsync<dynamic>(query).Result;

            if (!response.IsEmpty)
            {
                var bulkRequest = new BulkRequest();

                foreach (var row in response.Rows)
                    bulkRequest.Delete(row.Id, row.Value.rev.ToString());
                
                client.Documents.BulkAsync(bulkRequest).Wait();
            }
        }
    }
}