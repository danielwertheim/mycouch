using System;
using System.Collections.Generic;
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
        /// POSTs or PUTs an entity to the database.
        /// If ID is assigned in the Entity, it will perform
        /// a PUT.
        /// If NO ID is assigned in the Entity, it will perform
        /// a POST, and assign the DB GENERATED ID back to the entity.
        /// If you have assigned BOTH ID and REV, a PUT that updates
        /// the current document will be performed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> StoreAsync<T>(T entity) where T : class;

        /// <summary>
        /// NOTE, NOTE, NOTE! An underlying lookup of latest known REVISION
        /// will be performed, then that revision will be used to to overwrite
        /// an existing document with <paramref name="doc"/>.
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
        /// an existing document with <paramref name="entity"/>.
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
        /// Deletes a document by <paramref name="id"/>. It will perform an additional HEAD-request
        /// to lookup the value for latest known revision.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// It will perform an additional HEAD-request
        /// to lookup the value for latest known revision.
        /// </remarks>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Deletes a document by <paramref name="id"/> and <paramref name="rev"/>.
        /// If you do not know the <paramref name="rev"/> or just want to delete
        /// the latest know revision, use <see cref="DeleteAsync(string)"/>
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
        /// <param name="lookupRev">
        /// If true (default is false), an additional HEAD-request is performed
        /// to lookup the last known rev.</param>
        /// <remarks>
        /// If you know the current revision, ensure it is assigned in the entity
        /// and use false for <paramref name="lookupRev"/>,
        /// that will save you from an additional HEAD-request.
        /// </remarks>
        /// <returns></returns>
        Task<bool> DeleteAsync<TEntity>(TEntity entity, bool lookupRev = false) where TEntity : class;

        /// <summary>
        /// Issues a bulk delete of passed <see cref="DocumentHeader"/> in <paramref name="documents"/>.
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        Task<DeleteManyResult> DeleteManyAsync(params DocumentHeader[] documents);

        /// <summary>
        /// Checks for existance of a document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id, string rev = null);

        /// <summary>
        /// Returns the document header for a document by doing a HEAD-request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<DocumentHeader> GetHeaderAsync(string id, string rev = null);

        /// <summary>
        /// Returns documents headers matching sent <paramref name="ids"/>, via <paramref name="onResult"/>.
        /// It will query the all-docs view and return the id and ref via <see cref="DocumentHeader"/>.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        Task<QueryInfo> GetHeadersAsync(string[] ids, Action<DocumentHeader> onResult);

        /// <summary>
        /// Returns documents headers matching sent <paramref name="ids"/>.
        /// It will query the all-docs view and return the id and ref via <see cref="DocumentHeader"/>.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<DocumentHeader>> GetHeadersAsync(string[] ids);

        /// <summary>
        /// Returns a document by <param ref="id"/> and optinally a <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<string> GetByIdAsync(string id, string rev = null);

        /// <summary>
        /// Returns a document as an entity, by <paramref name="id"/> and
        /// optionally <paramref name="rev"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class;

        /// <summary>
        /// Returns documents matching sent <paramref name="ids"/>, via <paramref name="onResult"/>.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        Task<QueryInfo> GetByIdsAsync(string[] ids, Action<string> onResult);

        /// <summary>
        /// Returns entities matching sent <paramref name="ids"/>, via <paramref name="onResult"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        Task<QueryInfo> GetByIdsAsync<T>(string[] ids, Action<T> onResult) where T : class;

        /// <summary>
        /// Returns documents matching sent <paramref name="ids"/>.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetByIdsAsync(params string[] ids);

        /// <summary>
        /// Returns entities matching sent <paramref name="ids"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetByIdsAsync<T>(params string[] ids) where T : class;

        Task<QueryInfo> GetValueByKeysAsync(ViewIdentity view, object[] keys, Action<string> onResult);

        Task<QueryInfo> GetValueByKeysAsync<T>(ViewIdentity view, object[] keys, Action<T> onResult) where T : class;

        Task<IEnumerable<string>> GetValueByKeysAsync(ViewIdentity view, params object[] keys);

        Task<IEnumerable<T>> GetValueByKeysAsync<T>(ViewIdentity view, params object[] keys) where T : class;

        Task<QueryInfo> GetIncludedDocByKeysAsync(ViewIdentity view, object[] keys, Action<string> onResult);

        Task<QueryInfo> GetIncludedDocByKeysAsync<TValue>(ViewIdentity view, object[] keys, Action<TValue> onResult) where TValue : class;

        Task<IEnumerable<string>> GetIncludedDocByKeysAsync(ViewIdentity view, params object[] keys);

        Task<IEnumerable<TIncludedDoc>> GetIncludedDocByKeysAsync<TIncludedDoc>(ViewIdentity view, params object[] keys) where TIncludedDoc : class;

        Task<IEnumerable<Row>> QueryAsync(Query query);
        Task<IEnumerable<Row<TValue>>> QueryAsync<TValue>(Query query);
        Task<IEnumerable<Row<TValue, TIncludedDoc>>> QueryAsync<TValue, TIncludedDoc>(Query query);

        Task<QueryInfo> QueryAsync(Query query, Action<Row> onResult);
        Task<QueryInfo> QueryAsync<TValue>(Query query, Action<Row<TValue>> onResult);
        Task<QueryInfo> QueryAsync<TValue, TIncludedDoc>(Query query, Action<Row<TValue, TIncludedDoc>> onResult);
    }
}