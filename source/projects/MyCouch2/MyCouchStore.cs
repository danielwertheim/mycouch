using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
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
            Ensure.That(client, "client").IsNotNull();

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

            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.PostAsync(doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string doc)
        {
            ThrowIfDisposed();

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.PutAsync(id, doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string rev, string doc)
        {
            ThrowIfDisposed();

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.PutAsync(id, rev, doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<T> StoreAsync<T>(T entity) where T : class
        {
            ThrowIfDisposed();

            Ensure.That(entity, "entity").IsNotNull();

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

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var header = await GetHeaderAsync(id);

            return (header == null)
                ? await StoreAsync(id, doc).ForAwait()
                : await StoreAsync(header.Id, header.Rev, doc).ForAwait();
        }

        public virtual async Task<T> SetAsync<T>(T entity) where T : class
        {
            ThrowIfDisposed();

            Ensure.That(entity, "entity").IsNotNull();

            var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
            Ensure.That(id, "EntityId").IsNotNullOrWhiteSpace();

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

            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(newId, "newId").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.CopyAsync(srcId, newId).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string srcRev, string newId)
        {
            ThrowIfDisposed();

            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(srcRev, "srcRev").IsNotNullOrWhiteSpace();
            Ensure.That(newId, "newId").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.CopyAsync(srcId, srcRev, newId).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(trgId, "trgId").IsNotNullOrWhiteSpace();
            Ensure.That(trgRev, "trgRev").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.ReplaceAsync(srcId, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            Ensure.That(srcId, "srcId").IsNotNullOrWhiteSpace();
            Ensure.That(trgId, "trgId").IsNotNullOrWhiteSpace();
            Ensure.That(trgRev, "trgRev").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.ReplaceAsync(srcId, srcRev, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            ThrowIfDisposed();

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var head = await GetHeaderAsync(id).ForAwait();
            if (head == null)
                return false;

            return await DeleteAsync(id, head.Rev).ForAwait();
        }

        public virtual async Task<bool> DeleteAsync(string id, string rev)
        {
            ThrowIfDisposed();

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.DeleteAsync(id, rev).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<bool> DeleteAsync<TEntity>(TEntity entity, bool lookupRev = false) where TEntity : class
        {
            ThrowIfDisposed();

            Ensure.That(entity, "entity").IsNotNull();

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

            Ensure.That(documents, "documents").HasItems();

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

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return response.IsSuccess;
        }

        public virtual async Task<DocumentHeader> GetHeaderAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<QueryInfo> GetHeadersAsync(string[] ids, Action<DocumentHeader> onResult)
        {
            ThrowIfDisposed();

            Ensure.That(ids, "ids").HasItems();
            Ensure.That(onResult, "onResult").IsNotNull();

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

            Ensure.That(ids, "ids").HasItems();

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

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var response = await Client.Documents.GetAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class
        {
            ThrowIfDisposed();

            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

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

            Ensure.That(ids, "ids").HasItems();
            Ensure.That(onResult, "onResult").IsNotNull();

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

            Ensure.That(ids, "ids").HasItems();

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

            Ensure.That(view, "view").IsNotNull();
            Ensure.That(keys, "keys").HasItems();
            Ensure.That(onResult, "onResult").IsNotNull();

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

            Ensure.That(view, "view").IsNotNull();
            Ensure.That(keys, "keys").HasItems();

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

            Ensure.That(view, "view").IsNotNull();
            Ensure.That(keys, "keys").HasItems();
            Ensure.That(onResult, "onResult").IsNotNull();

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

            Ensure.That(view, "view").IsNotNull();
            Ensure.That(keys, "keys").HasItems();

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<string, TIncludedDoc>(request).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Where(r => r.IncludedDoc != null).Select(r => r.IncludedDoc);
        }

        public virtual async Task<IEnumerable<Row>> QueryAsync(Query query)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            var response = await Client.Views.QueryAsync(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<IEnumerable<Row<TValue>>> QueryAsync<TValue>(Query query)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            var response = await Client.Views.QueryAsync<TValue>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row<TValue>(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<IEnumerable<Row<TValue, TIncludedDoc>>> QueryAsync<TValue, TIncludedDoc>(Query query)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row<TValue, TIncludedDoc>(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<QueryInfo> QueryAsync(Query query, Action<Row> onResult)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            var response = await Client.Views.QueryAsync(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response);
        }

        public virtual async Task<QueryInfo> QueryAsync<TValue>(Query query, Action<Row<TValue>> onResult)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            var response = await Client.Views.QueryAsync<TValue>(query.ToRequest()).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row<TValue>(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response);
        }

        public virtual async Task<QueryInfo> QueryAsync<TValue, TIncludedDoc>(Query query, Action<Row<TValue, TIncludedDoc>> onResult)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

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

#if net45
        [Serializable]
#endif
        private class AllDocsValue
        {
            public string Rev { get; set; }
            public bool Deleted { get; set; }
        }
    }
}