using EnsureThat;

namespace MyCouch.Requests
{
    public class PostEntityRequest<T> : Request where T : class
    {
        public bool Batch { get; set; }
        public T Entity { get; set; }

        public PostEntityRequest(T entity)
        {
            EnsureArg.IsNotNull(entity, nameof(entity));

            Batch = false;
            Entity = entity;
        }
    }
}