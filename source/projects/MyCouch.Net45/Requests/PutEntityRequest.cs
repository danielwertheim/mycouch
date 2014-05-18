using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class PutEntityRequest<T> : Request where T : class
    {
        public bool Batch { get; set; }
        public T Entity { get; set; }

        public PutEntityRequest(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Batch = false;
            Entity = entity;
        }
    }
}