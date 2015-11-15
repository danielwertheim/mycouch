using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Requests;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DatabaseHeaderResponseFactory : ResponseFactoryBase
    {
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public DatabaseHeaderResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<DatabaseHeaderResponse> CreateAsync(DatabaseRequest request, HttpResponseMessage httpResponse)
        {
            return await MaterializeAsync<DatabaseHeaderResponse>(
                httpResponse,
                (r1, r2) =>
                {
                    r1.DbName = request.DbName;
                    return Task.FromResult(true);
                },
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }
    }
}