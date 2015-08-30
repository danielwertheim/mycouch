using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
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
            Ensure.That(serializer, "serializer").IsNotNull();

            ReplicationResponseFactory = new ReplicationResponseFactory(serializer);
            ReplicateDatabaseHttpRequestFactory = new ReplicateDatabaseServerHttpRequestFactory(serializer);
        }

        public virtual Task<ReplicationResponse> ReplicateAsync(string id, string source, string target)
        {
            return ReplicateAsync(new ReplicateDatabaseRequest(id, source, target));
        }

        public virtual async Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessReplicationResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(ReplicateDatabaseRequest request)
        {
            return ReplicateDatabaseHttpRequestFactory.Create(request);
        }

        protected virtual ReplicationResponse ProcessReplicationResponse(HttpResponseMessage response)
        {
            return ReplicationResponseFactory.Create(response);
        }
    }
}