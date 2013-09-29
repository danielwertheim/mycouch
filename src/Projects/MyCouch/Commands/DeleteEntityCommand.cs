using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DeleteEntityCommand<T> : ICommand where T : class
    {
        public T Entity { get; set; }

        public DeleteEntityCommand(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Entity = entity;
        }
    }
}