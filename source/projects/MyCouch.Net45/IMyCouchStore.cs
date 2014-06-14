using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace MyCouch
{
    /// <summary>
    /// A somewhat opinionated abstraction over MyCouch which removes the
    /// use of Http-responses and lets you work directly with the entities.
    /// Also, provides you with queries via observables.
    /// If a non successful operation, an exception is thrown.
    /// </summary>
    public interface IMyCouchStore : IDisposable
    {
        /// <summary>
        /// The underlying db-client.
        /// </summary>
        IMyCouchClient Client { get; }

        /// <summary>
        /// The Scheduler used when running observable queries.
        /// By default <see cref="TaskPoolScheduler.Default"/>.
        /// </summary>
        Func<IScheduler> ObservableSubscribeOnScheduler { set; }

        /// <summary>
        /// POSTs a raw NEW document to the database.
        /// Should be used to POST a NEW document
        /// where you let the database generate the
        /// ID.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeader> StoreAsync(string doc);

        /// <summary>
        /// PUTs a raw NEW document to the database.
        /// Should be used to PUT a NEW document
        /// where you generate the ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeader> StoreAsync(string id, string doc);

        /// <summary>
        /// PUTs a raw EXISTING document to the database.
        /// Should be used to PUT a new version of an
        /// EXISTING document in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeader> StoreAsync(string id, string rev, string doc);

        /// <summary>
        /// POSTs or PUTs a NEW entity to the database.
        /// If ID is assigned in the Entity, it will perform
        /// a PUT.
        /// If NO ID is assigned in the Entity, it will perform
        /// a POST, and assign the DB GENERATED ID back to the entity.
        /// Should be used to insert new entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> StoreAsync<T>(T entity) where T : class;

        /// <summary>
        /// NOTE, NOTE, NOTE! An underlying lookup of latest known REVISION
        /// will be performed, then that revision will be used to to overwrite
        /// an existing document with <see cref="doc"/>.
        /// </summary>
        /// <remarks>
        /// An initial HEAD will be performed to lookup the current revision.
        /// If you KNOW the revision, use <see cref="StoreAsync(string, string, string)"/> instead.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeader> SetAsync(string id, string doc);

        /// <summary>
        /// NOTE, NOTE, NOTE! An underlying lookup of latest known REVISION
        /// will be performed, then that revision will be used to to overwrite
        /// an existing document with <see cref="entity"/>.
        /// </summary>
        /// <remarks>
        /// An initial HEAD will be performed to lookup the current revision.
        /// If you KNOW that the revision is allready assigned, use <see cref="StoreAsync{T}(T)"/> instead.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> SetAsync<T>(T entity) where T : class;

        /// <summary>
        /// Makes a copy of an existing document to a document with a new id.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        Task<DocumentHeader> CopyAsync(string srcId, string newId);

        /// <summary>
        /// Makes a copy of an existing revision of a document to a document with a new id.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        Task<DocumentHeader> CopyAsync(string srcId, string srcRev, string newId);

        /// <summary>
        /// Replaces an existing document, with another existing document.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <returns></returns>
        Task<DocumentHeader> ReplaceAsync(string srcId, string trgId, string trgRev);

        /// <summary>
        /// Replaces an existing document, with another existing revision of a document.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <returns></returns>
        Task<DocumentHeader> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev);

        /// <summary>
        /// Deletes a document by <see cref="id"/> and <see cref="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id, string rev);

        /// <summary>
        /// Deletes a document by extracting id and rev from sent entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Checks for existance of a document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id, string rev = null);

        /// <summary>
        /// Returns the document header for a document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<DocumentHeader> GetHeaderAsync(string id, string rev = null);

        /// <summary>
        /// Returns a document by <see cref="id"/> and optinally a <see cref="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<string> GetByIdAsync(string id, string rev = null);

        /// <summary>
        /// Returns a document as an entity, by <see cref="id"/> and
        /// optionally <see cref="rev"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class;

        IObservable<Row> Query(Query query);
        IObservable<Row<TValue>> Query<TValue>(Query query);
        IObservable<Row<TValue, TIncludedDoc>> Query<TValue, TIncludedDoc>(Query query);

        Task<QueryInfo> QueryAsync(Query query, Action<Row> onResult);
        Task<QueryInfo> QueryAsync<TValue>(Query query, Action<Row<TValue>> onResult);
        Task<QueryInfo> QueryAsync<TValue, TIncludedDoc>(Query query, Action<Row<TValue, TIncludedDoc>> onResult);
    }
}