using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PutEntityCommand<T> : ICommand where T : class
    {
        public T Entity { get; set; }

        public PutEntityCommand(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Entity = entity;
        }
    }
}