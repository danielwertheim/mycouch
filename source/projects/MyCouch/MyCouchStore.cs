using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Querying;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public class MyCouchStore : IMyCouchStore
    {
        protected bool IsDisposed { get; private set; }

        public IMyCouchClient Client { get; protected set; }

        public MyCouchStore(string serverAddress, string dbName = null) : this(new MyCouchClient(serverAddress, dbName)) { }

        public MyCouchStore(Uri serverAddress, string dbName = null) : this(new MyCouchClient(serverAddress, dbName)) { }

        public MyCouchStore(IMyCouchClient client)
        {
            EnsureArg.IsNotNull(client, nameof(client));

            Client = client;
            IsDisposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || !disposing)
                return;

            Client.Dispose();
            Client = null;
        }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string doc)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var response = await Client.Documents.PostAsync(doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string doc)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var response = await Client.Documents.PutAsync(id, doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string rev, string doc)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(rev, nameof(rev));
            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var response = await Client.Documents.PutAsync(id, rev, doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<T> StoreAsync<T>(T entity) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(entity, nameof(entity));

            var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
            var response = string.IsNullOrEmpty(id)
                ? await Client.Entities.PostAsync(entity).ForAwait()
                : await Client.Entities.PutAsync(entity).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<DocumentHeader> SetAsync(string id, string doc)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var header = await GetHeaderAsync(id);

            return (header == null)
                ? await StoreAsync(id, doc).ForAwait()
                : await StoreAsync(header.Id, header.Rev, doc).ForAwait();
        }

        public virtual async Task<T> SetAsync<T>(T entity) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(entity, nameof(entity));

            var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException($"EntityId could not be extracted. Ensure member exists on type: '{typeof(T).Name}'.", nameof(entity));

            var header = await GetHeaderAsync(id).ForAwait();
            if (header != null)
                Client.Entities.Reflector.RevMember.SetValueTo(entity, header.Rev);

            var response = await Client.Entities.PutAsync(entity).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return entity;
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string newId)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(newId, nameof(newId));

            var response = await Client.Documents.CopyAsync(srcId, newId).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string srcRev, string newId)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(srcRev, nameof(srcRev));
            EnsureArg.IsNotNullOrWhiteSpace(newId, nameof(newId));

            var response = await Client.Documents.CopyAsync(srcId, srcRev, newId).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(trgId, nameof(trgId));
            EnsureArg.IsNotNullOrWhiteSpace(trgRev, nameof(trgRev));

            var response = await Client.Documents.ReplaceAsync(srcId, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(trgId, nameof(trgId));
            EnsureArg.IsNotNullOrWhiteSpace(trgRev, nameof(trgRev));

            var response = await Client.Documents.ReplaceAsync(srcId, srcRev, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var head = await GetHeaderAsync(id).ForAwait();
            if (head == null)
                return false;

            return await DeleteAsync(id, head.Rev).ForAwait();
        }

        public virtual async Task<bool> DeleteAsync(string id, string rev)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(rev, nameof(rev));

            var response = await Client.Documents.DeleteAsync(id, rev).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<bool> DeleteAsync<TEntity>(TEntity entity, bool lookupRev = false) where TEntity : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(entity, nameof(entity));

            if (lookupRev)
            {
                var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
                var head = await GetHeaderAsync(id).ForAwait();
                if (head == null)
                    return false;

                Client.Entities.Reflector.RevMember.SetValueTo(entity, head.Rev);
            }

            var response = await Client.Entities.DeleteAsync(entity).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<DeleteManyResult> DeleteManyAsync(params DocumentHeader[] documents)
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(documents, nameof(documents));

            var request = new BulkRequest()
                .Delete(documents);

            var response = await Client.Documents.BulkAsync(request);

            ThrowIfNotSuccessfulResponse(response);

            return new DeleteManyResult
            {
                Rows = response.Rows.Select(r => new DeleteManyResult.Row
                {
                    Id = r.Id,
                    Rev = r.Rev,
                    Error = r.Error,
                    Reason = r.Reason,
                    Deleted = r.Succeeded
                }).ToArray()
            };
        }

        public virtual async Task<bool> ExistsAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return response.IsSuccess;
        }

        public virtual async Task<DocumentHeader> GetHeaderAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<QueryInfo> GetHeadersAsync(string[] ids, Action<DocumentHeader> onResult)
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids));
            var response = await Client.Views.QueryAsync<AllDocsValue>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.Id != null && AllDocsItemRepresentsActualExistingDoc(r.Value)))
                onResult(new DocumentHeader(row.Id, row.Value.Rev));

            return CreateQueryInfoFrom(response);
        }

        public virtual async Task<IEnumerable<DocumentHeader>> GetHeadersAsync(string[] ids)
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids));
            var response = await Client.Views.QueryAsync<AllDocsValue>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows
                .Where(r => r.Id != null && AllDocsItemRepresentsActualExistingDoc(r.Value))
                .Select(r => new DocumentHeader(r.Id, r.Value.Rev));
        }

        public virtual async Task<string> GetByIdAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Documents.GetAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Entities.GetAsync<TEntity>(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual Task<QueryInfo> GetByIdsAsync(string[] ids, Action<string> onResult)
        {
            ThrowIfDisposed();

            return GetByIdsAsync<string>(ids, onResult);
        }

        public virtual async Task<QueryInfo> GetByIdsAsync<T>(string[] ids, Action<T> onResult) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<AllDocsValue, T>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.Id != null && r.IncludedDoc != null && AllDocsItemRepresentsActualExistingDoc(r.Value)))
                onResult(row.IncludedDoc);

            return CreateQueryInfoFrom(response);
        }

        public virtual Task<IEnumerable<string>> GetByIdsAsync(params string[] ids)
        {
            return GetByIdsAsync<string>(ids);
        }

        public virtual async Task<IEnumerable<T>> GetByIdsAsync<T>(params string[] ids) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<AllDocsValue, T>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows
                .Where(r => r.Id != null && r.IncludedDoc != null && AllDocsItemRepresentsActualExistingDoc(r.Value))
                .Select(r => r.IncludedDoc);
        }

        private bool AllDocsItemRepresentsActualExistingDoc(AllDocsValue value)
        {
            return value != null && !value.Deleted;
        }

        public virtual Task<QueryInfo> GetValueByKeysAsync(ViewIdentity view, object[] keys, Action<string> onResult)
        {
            ThrowIfDisposed();

            return GetValueByKeysAsync<string>(view, keys, onResult);
        }

        public virtual async Task<QueryInfo> GetValueByKeysAsync<TValue>(ViewIdentity view, object[] keys, Action<TValue> onResult) where TValue : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys));
            var response = await Client.Views.QueryAsync<TValue>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.Value != null))
                onResult(row.Value);

            return CreateQueryInfoFrom(response);
        }

        public virtual Task<IEnumerable<string>> GetValueByKeysAsync(ViewIdentity view, params object[] keys)
        {
            return GetValueByKeysAsync<string>(view, keys);
        }

        public virtual async Task<IEnumerable<TValue>> GetValueByKeysAsync<TValue>(ViewIdentity view, params object[] keys) where TValue : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys));
            var response = await Client.Views.QueryAsync<TValue>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Where(r => r.Value != null).Select(r => r.Value);
        }

        public virtual Task<QueryInfo> GetIncludedDocByKeysAsync(ViewIdentity view, object[] keys, Action<string> onResult)
        {
            ThrowIfDisposed();

            return GetIncludedDocByKeysAsync<string>(view, keys, onResult);
        }

        public virtual async Task<QueryInfo> GetIncludedDocByKeysAsync<TIncludedDoc>(ViewIdentity view, object[] keys, Action<TIncludedDoc> onResult) where TIncludedDoc : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<string, TIncludedDoc>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.IncludedDoc != null))
                onResult(row.IncludedDoc);

            return CreateQueryInfoFrom(response);
        }

        public virtual Task<IEnumerable<string>> GetIncludedDocByKeysAsync(ViewIdentity view, params object[] keys)
        {
            return GetIncludedDocByKeysAsync<string>(view, keys);
        }

        public virtual async Task<IEnumerable<TIncludedDoc>> GetIncludedDocByKeysAsync<TIncludedDoc>(ViewIdentity view, params object[] keys) where TIncludedDoc : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<string, TIncludedDoc>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Where(r => r.IncludedDoc != null).Select(r => r.IncludedDoc);
        }

        public virtual async Task<IEnumerable<Row>> QueryAsync(Query query)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<IEnumerable<Row<TValue>>> QueryAsync<TValue>(Query query)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row<TValue>(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<IEnumerable<Row<TValue, TIncludedDoc>>> QueryAsync<TValue, TIncludedDoc>(Query query)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row<TValue, TIncludedDoc>(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<QueryInfo> QueryAsync(Query query, Action<Row> onResult)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response);
        }

        public virtual async Task<QueryInfo> QueryAsync<TValue>(Query query, Action<Row<TValue>> onResult)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row<TValue>(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response);
        }

        public virtual async Task<QueryInfo> QueryAsync<TValue, TIncludedDoc>(Query query, Action<Row<TValue, TIncludedDoc>> onResult)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row<TValue, TIncludedDoc>(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response);
        }

        protected virtual QueryInfo CreateQueryInfoFrom<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response)
        {
            return new QueryInfo(response.TotalRows, response.RowCount, response.OffSet, response.UpdateSeq);
        }

        protected virtual void ThrowIfNotSuccessfulResponse(Response response)
        {
            if (response.IsSuccess)
                return;

            throw new MyCouchResponseException(response);
        }

        private class AllDocsValue
        {
            public string Rev { get; set; }
            public bool Deleted { get; set; }
        }
    }
}