using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
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