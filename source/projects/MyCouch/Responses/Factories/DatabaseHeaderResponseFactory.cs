using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
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
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<DatabaseHeaderResponse> CreateAsync(DatabaseRequest request, HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<DatabaseHeaderResponse>(
                httpResponse,
                (r1, r2) =>
                {
                    r1.DbName = request.DbName;
                    return Task.FromResult(true);
                },
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}