using EnsureThat;

namespace MyCouch.Requests
{
    public class PurgeEntityRequest<T> : Request where T : class
    {
        public T Entity { get; set; }

        public PurgeEntityRequest(T entity)
        {
            EnsureArg.IsNotNull(entity, nameof(entity));

            Entity = entity;
        }
    }
}