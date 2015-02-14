using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class PutEntityRequest<T> : Request where T : class
    {
        /// <summary>
        /// If provided, then this value will be used
        /// as document id (_id) instead of value in the entity.
        /// </summary>
        public string ExplicitId { get; private set; }

        /// <summary>
        /// If provided, then this value will be used
        /// as document rev (_rev) instead of value in the entity.
        /// </summary>
        public string ExplicitRev { get; private set; }

        public bool Batch { get; set; }
        public T Entity { get; private set; }

        /// <summary>
        /// Initialized with <see cref="ExplicitId"/>, hence document Id
        /// will NOT be extracted from <paramref name="entity"/>
        /// </summary>
        /// <param name="id">Used as explicit id instead of extracting from entity.</param>
        /// <param name="entity"></param>
        public PutEntityRequest(string id, T entity)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(entity, "entity").IsNotNull();

            Initialize(id, null, entity);
        }

        /// <summary>
        /// Initialized with <see cref="ExplicitId"/> and <see cref="ExplicitRev"/>,
        /// hence document Id and Rev will NOT be extracted from <paramref name="entity"/>
        /// </summary>
        /// <param name="id">Used as explicit id instead of extracting it from the entity.</param>
        /// <param name="rev">Used as explicit rev instead of extracting it from the entity.</param>
        /// <param name="entity"></param>
        public PutEntityRequest(string id, string rev, T entity)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();
            Ensure.That(entity, "entity").IsNotNull();

            Initialize(id, rev, entity);
        }

        /// <summary>
        /// Document Id and Rev will be extracted from entity
        /// when persisting.
        /// </summary>
        /// <param name="entity"></param>
        public PutEntityRequest(T entity)
        {
            Ensure.That(entity, "entity").IsNotNull();

            Batch = false;
            Entity = entity;

            Initialize(null, null, entity);
        }

        private void Initialize(string id, string rev, T entity)
        {
            ExplicitId = id ?? string.Empty;
            ExplicitRev = rev ?? string.Empty;
            Batch = false;
            Entity = entity;
        }
    }
}