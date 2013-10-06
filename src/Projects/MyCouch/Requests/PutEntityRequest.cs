using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PutEntityRequest<T> : IRequest where T : class
    {
        public T Entity { get; set; }

        public PutEntityRequest(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Entity = entity;
        }
    }
}