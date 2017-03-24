using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class DeleteEntityRequest<T> : Request where T : class
    {
        public T Entity { get; set; }

        public DeleteEntityRequest(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Entity = entity;
        }
    }
}