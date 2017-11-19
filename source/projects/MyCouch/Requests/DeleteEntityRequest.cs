using EnsureThat;

namespace MyCouch.Requests
{
    public class DeleteEntityRequest<T> : Request where T : class
    {
        public T Entity { get; set; }

        public DeleteEntityRequest(T entity)
        {
            EnsureArg.IsNotNull(entity, nameof(entity));

            Entity = entity;
        }
    }
}