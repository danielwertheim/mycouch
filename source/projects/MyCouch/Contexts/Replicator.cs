using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Replicator : ApiContextBase<IServerConnection>, IReplicator
    {
        protected ReplicationResponseFactory ReplicationResponseFactory { get; set; }
        protected ReplicateDatabaseServerHttpRequestFactory ReplicateDatabaseHttpRequestFactory { get; set; }

        public Replicator(IServerConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            ReplicationResponseFactory = new ReplicationResponseFactory(serializer);
            ReplicateDatabaseHttpRequestFactory = new ReplicateDatabaseServerHttpRequestFactory(serializer);
        }

        public virtual Task<ReplicationResponse> ReplicateAsync(string id, string source, string target)
        {
            return ReplicateAsync(new ReplicateDatabaseRequest(id, source, target));
        }

        public virtual async Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request)
        {
            var httpRequest = ReplicateDatabaseHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await ReplicationResponseFactory.CreateAsync(res).ForAwait();
            }
        }
    }
}