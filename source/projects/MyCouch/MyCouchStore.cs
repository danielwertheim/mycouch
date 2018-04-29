using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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

        public virtual async Task<DocumentHeader> StoreAsync(string doc, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var response = await Client.Documents.PostAsync(doc, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string doc, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var response = await Client.Documents.PutAsync(id, doc, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string rev, string doc, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(rev, nameof(rev));
            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var response = await Client.Documents.PutAsync(id, rev, doc, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<T> StoreAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(entity, nameof(entity));

            var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
            var response = string.IsNullOrEmpty(id)
                ? await Client.Entities.PostAsync(entity, cancellationToken).ForAwait()
                : await Client.Entities.PutAsync(entity, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<DocumentHeader> SetAsync(string id, string doc, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(doc, nameof(doc));

            var header = await GetHeaderAsync(id, null, cancellationToken);

            return (header == null)
                ? await StoreAsync(id, doc, cancellationToken).ForAwait()
                : await StoreAsync(header.Id, header.Rev, doc, cancellationToken).ForAwait();
        }

        public virtual async Task<T> SetAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(entity, nameof(entity));

            var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException($"EntityId could not be extracted. Ensure member exists on type: '{typeof(T).Name}'.", nameof(entity));

            var header = await GetHeaderAsync(id, null, cancellationToken).ForAwait();
            if (header != null)
                Client.Entities.Reflector.RevMember.SetValueTo(entity, header.Rev);

            var response = await Client.Entities.PutAsync(entity, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return entity;
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string newId, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(newId, nameof(newId));

            var response = await Client.Documents.CopyAsync(srcId, newId, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string srcRev, string newId, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(srcRev, nameof(srcRev));
            EnsureArg.IsNotNullOrWhiteSpace(newId, nameof(newId));

            var response = await Client.Documents.CopyAsync(srcId, srcRev, newId, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string trgId, string trgRev, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(trgId, nameof(trgId));
            EnsureArg.IsNotNullOrWhiteSpace(trgRev, nameof(trgRev));

            var response = await Client.Documents.ReplaceAsync(srcId, trgId, trgRev, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(srcId, nameof(srcId));
            EnsureArg.IsNotNullOrWhiteSpace(trgId, nameof(trgId));
            EnsureArg.IsNotNullOrWhiteSpace(trgRev, nameof(trgRev));

            var response = await Client.Documents.ReplaceAsync(srcId, srcRev, trgId, trgRev, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var head = await GetHeaderAsync(id, null, cancellationToken).ForAwait();
            if (head == null)
                return false;

            return await DeleteAsync(id, head.Rev, cancellationToken).ForAwait();
        }

        public virtual async Task<bool> DeleteAsync(string id, string rev, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));
            EnsureArg.IsNotNullOrWhiteSpace(rev, nameof(rev));

            var response = await Client.Documents.DeleteAsync(id, rev, cancellationToken).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<bool> DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(entity, nameof(entity));

            var rev = Client.Entities.Reflector.RevMember.GetValueFrom(entity);
            if (string.IsNullOrWhiteSpace(rev))
            {
                var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
                var head = await GetHeaderAsync(id, null, cancellationToken).ForAwait();
                if (head == null)
                    return false;

                Client.Entities.Reflector.RevMember.SetValueTo(entity, head.Rev);
            }

            var response = await Client.Entities.DeleteAsync(entity, cancellationToken).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<DeleteManyResult> DeleteManyAsync(DocumentHeader[] documents, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(documents, nameof(documents));

            var request = new BulkRequest()
                .Delete(documents);

            var response = await Client.Documents.BulkAsync(request, cancellationToken);

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

        public virtual async Task<bool> ExistsAsync(string id, string rev = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Documents.HeadAsync(id, rev, cancellationToken).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return response.IsSuccess;
        }

        public virtual async Task<DocumentHeader> GetHeaderAsync(string id, string rev = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Documents.HeadAsync(id, rev, cancellationToken).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<QueryInfo> GetHeadersAsync(string[] ids, Action<DocumentHeader> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids));
            var response = await Client.Views.QueryAsync<AllDocsValue>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.Id != null && AllDocsItemRepresentsActualExistingDoc(r.Value)))
                onResult(new DocumentHeader(row.Id, row.Value.Rev));

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        public virtual async Task<IEnumerable<DocumentHeader>> GetHeadersAsync(string[] ids, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids));
            var response = await Client.Views.QueryAsync<AllDocsValue>(request,cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows
                .Where(r => r.Id != null && AllDocsItemRepresentsActualExistingDoc(r.Value))
                .Select(r => new DocumentHeader(r.Id, r.Value.Rev));
        }

        public virtual async Task<string> GetByIdAsync(string id, string rev = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Documents.GetAsync(id, rev, cancellationToken).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null, CancellationToken cancellationToken = default) where TEntity : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var response = await Client.Entities.GetAsync<TEntity>(id, rev, cancellationToken).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual Task<QueryInfo> GetByIdsAsync(string[] ids, Action<string> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return GetByIdsAsync<string>(ids, onResult, cancellationToken);
        }

        public virtual async Task<QueryInfo> GetByIdsAsync<T>(string[] ids, Action<T> onResult, CancellationToken cancellationToken = default) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<AllDocsValue, T>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.Id != null && r.IncludedDoc != null && AllDocsItemRepresentsActualExistingDoc(r.Value)))
                onResult(row.IncludedDoc);

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        public Task<IEnumerable<string>> GetByIdsAsync(string[] ids, CancellationToken cancellationToken = default)
        {
            return GetByIdsAsync<string>(ids, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetByIdsAsync<T>(string[] ids, CancellationToken cancellationToken = default) where T : class
        {
            ThrowIfDisposed();

            EnsureArg.HasItems(ids, nameof(ids));

            var request = new QueryViewRequest(SystemViewIdentity.AllDocs).Configure(r => r.Keys(ids).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<AllDocsValue, T>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows
                .Where(r => r.Id != null && r.IncludedDoc != null && AllDocsItemRepresentsActualExistingDoc(r.Value))
                .Select(r => r.IncludedDoc);
        }

        private bool AllDocsItemRepresentsActualExistingDoc(AllDocsValue value)
        {
            return value != null && !value.Deleted;
        }

        public virtual Task<QueryInfo> GetValueByKeysAsync(ViewIdentity view, object[] keys, Action<string> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return GetValueByKeysAsync<string>(view, keys, onResult, cancellationToken);
        }

        public virtual async Task<QueryInfo> GetValueByKeysAsync<TValue>(ViewIdentity view, object[] keys, Action<TValue> onResult, CancellationToken cancellationToken = default) where TValue : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys));
            var response = await Client.Views.QueryAsync<TValue>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.Value != null))
                onResult(row.Value);

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        public virtual Task<IEnumerable<string>> GetValueByKeysAsync(ViewIdentity view, object[] keys, CancellationToken cancellationToken = default)
        {
            return GetValueByKeysAsync<string>(view, keys, cancellationToken);
        }

        public virtual async Task<IEnumerable<TValue>> GetValueByKeysAsync<TValue>(ViewIdentity view, object[] keys, CancellationToken cancellationToken = default)
            where TValue : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys));
            var response = await Client.Views.QueryAsync<TValue>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Where(r => r.Value != null).Select(r => r.Value);
        }

        public virtual Task<QueryInfo> GetIncludedDocByKeysAsync(ViewIdentity view, object[] keys, Action<string> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return GetIncludedDocByKeysAsync<string>(view, keys, onResult, cancellationToken);
        }

        public virtual async Task<QueryInfo> GetIncludedDocByKeysAsync<TIncludedDoc>(ViewIdentity view, object[] keys, Action<TIncludedDoc> onResult, CancellationToken cancellationToken = default) where TIncludedDoc : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));
            EnsureArg.IsNotNull(onResult, nameof(onResult));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<string, TIncludedDoc>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows.Where(r => r.IncludedDoc != null))
                onResult(row.IncludedDoc);

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        public virtual Task<IEnumerable<string>> GetIncludedDocByKeysAsync(ViewIdentity view, object[] keys, CancellationToken cancellationToken = default)
        {
            return GetIncludedDocByKeysAsync<string>(view, keys, cancellationToken);
        }

        public virtual async Task<IEnumerable<TIncludedDoc>> GetIncludedDocByKeysAsync<TIncludedDoc>(ViewIdentity view, object[] keys, CancellationToken cancellationToken = default)
            where TIncludedDoc : class
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(view, nameof(view));
            EnsureArg.HasItems(keys, nameof(keys));

            var request = new QueryViewRequest(view).Configure(r => r.Keys(keys).IncludeDocs(true));
            var response = await Client.Views.QueryAsync<string, TIncludedDoc>(request, cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Where(r => r.IncludedDoc != null).Select(r => r.IncludedDoc);
        }

        public virtual async Task<IEnumerable<Row>> QueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync(query.ToRequest(), cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<IEnumerable<Row<TValue>>> QueryAsync<TValue>(Query query, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue>(query.ToRequest(), cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row<TValue>(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<IEnumerable<Row<TValue, TIncludedDoc>>> QueryAsync<TValue, TIncludedDoc>(Query query, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query.ToRequest(), cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Rows.Select(r => new Row<TValue, TIncludedDoc>(r.Id, r.Key, r.Value, r.IncludedDoc));
        }

        public virtual async Task<QueryInfo> QueryAsync(Query query, Action<Row> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync(query.ToRequest(), cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        public virtual async Task<QueryInfo> QueryAsync<TValue>(Query query, Action<Row<TValue>> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue>(query.ToRequest(), cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row<TValue>(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        public virtual async Task<QueryInfo> QueryAsync<TValue, TIncludedDoc>(Query query, Action<Row<TValue, TIncludedDoc>> onResult, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            EnsureArg.IsNotNull(query, nameof(query));

            var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query.ToRequest(), cancellationToken).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            foreach (var row in response.Rows)
                onResult(new Row<TValue, TIncludedDoc>(row.Id, row.Key, row.Value, row.IncludedDoc));

            return CreateQueryInfoFrom(response, cancellationToken);
        }

        protected virtual QueryInfo CreateQueryInfoFrom<TValue, TIncludedDoc>(ViewQueryResponse<TValue, TIncludedDoc> response, CancellationToken cancellationToken = default)
        {
            return new QueryInfo(response.TotalRows, response.RowCount, response.OffSet, response.UpdateSeq);
        }

        protected virtual void ThrowIfNotSuccessfulResponse(Response response)
        {
            if (response.IsSuccess)
                return;

            throw new MyCouchResponseException(response);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class AllDocsValue
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Rev { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public bool Deleted { get; set; }
        }
    }
}