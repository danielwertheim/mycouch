using System;
using EnsureThat;

namespace MyCouch.Commands
{
    [Serializable]
    public class PostEntityCommand<T> : IMyCouchCommand where T : class
    {
        public T Entity { get; set; }

        public PostEntityCommand(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Entity = entity;
        }
    }
}