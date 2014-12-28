using System.Net.Http;
using EnsureThat;
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

        public virtual DatabaseHeaderResponse Create(DatabaseRequest request, HttpResponseMessage httpResponse)
        {
            return Materialize<DatabaseHeaderResponse>(
                httpResponse,
                (r1, r2) =>
                {
                    r1.DbName = request.DbName;
                },
                FailedResponseMaterializer.Materialize);
        }
    }
}