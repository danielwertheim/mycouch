using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
    public class PostEntityRequest<T> : Request where T : class
    {
        public bool Batch { get; set; }
        public T Entity { get; set; }

        public PostEntityRequest(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Batch = false;
            Entity = entity;
        }
    }
}