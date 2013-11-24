using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch.IntegrationTests
{
    internal static class ClientExtensions
    {
        internal static void ClearAllDocuments(this IClient client)
        {
            var query = new QuerySystemViewRequest("_all_docs").Configure(q => q.Stale(Stale.UpdateAfter));
            var response = client.Views.QueryAsync<dynamic>(query).Result;

            BulkDelete(client, response);

            response = client.Views.QueryAsync<dynamic>(query).Result;

            BulkDelete(client, response);
        }

        private static void BulkDelete(IClient client, ViewQueryResponse<dynamic> response)
        {
            if (response.IsEmpty)
                return;

            var bulkRequest = new BulkRequest();

            foreach (var row in response.Rows)
                bulkRequest.Delete(row.Id, row.Value.rev.ToString());

            client.Documents.BulkAsync(bulkRequest).Wait();
        }
    }
}