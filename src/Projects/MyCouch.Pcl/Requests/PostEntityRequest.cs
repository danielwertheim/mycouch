using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class PostEntityRequest<T> : Request where T : class
    {
        public T Entity { get; set; }

        public PostEntityRequest(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Entity = entity;
        }
    }
}