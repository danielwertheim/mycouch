using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Querying;
using MyCouch.Responses;

namespace MyCouch
{
    public class MyCouchStore : IMyCouchStore
    {
        protected bool IsDisposed { get; private set; }

        public IMyCouchClient Client { get; protected set; }

        public Func<IScheduler> ObservableSubscribeOnScheduler { protected get; set; }

        public MyCouchStore(string dbUri, string dbName = null) : this(new MyCouchClient(dbUri, dbName)) { }

        public MyCouchStore(Uri dbUri, string dbName = null) : this(new MyCouchClient(dbUri, dbName)) { }

        public MyCouchStore(IMyCouchClient client)
        {
            Ensure.That(client, "client").IsNotNull();

            Client = client;
            IsDisposed = false;
            ObservableSubscribeOnScheduler = () => TaskPoolScheduler.Default;
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

            var response = await Client.Documents.PostAsync(doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string doc)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.PutAsync(id, doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> StoreAsync(string id, string rev, string doc)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.PutAsync(id, rev, doc).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<T> StoreAsync<T>(T entity) where T : class
        {
            Ensure.That(entity, "entity").IsNotNull();

            ThrowIfDisposed();

            var id = Client.Entities.Reflector.IdMember.GetValueFrom(entity);
            var response = string.IsNullOrEmpty(id)
                ? await Client.Entities.PostAsync(entity).ForAwait()
                : await Client.Entities.PutAsync(entity).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string newId)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.CopyAsync(srcId, newId).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> CopyAsync(string srcId, string srcRev, string newId)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.CopyAsync(srcId, srcRev, newId).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.ReplaceAsync(srcId, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<DocumentHeader> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.ReplaceAsync(srcId, srcRev, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<bool> DeleteAsync(string id, string rev)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.DeleteAsync(id, rev).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            ThrowIfDisposed();

            var response = await Client.Entities.DeleteAsync(entity).ForAwait();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return true;
        }

        public virtual async Task<bool> ExistsAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse(response);

            return response.IsSuccess;
        }

        public virtual async Task<DocumentHeader> GetHeaderAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<string> GetByIdAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.GetAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class
        {
            ThrowIfDisposed();

            var response = await Client.Entities.GetAsync<TEntity>(id, rev);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            ThrowIfNotSuccessfulResponse(response);

            return response.Content;
        }

        public virtual IObservable<Row> Query(Query query)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            return Observable.Create<Row>(async o =>
            {
                var response = await Client.Views.QueryAsync(query.ToRequest()).ForAwait();

                ThrowIfNotSuccessfulResponse(response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            }).SubscribeOn(ObservableSubscribeOnScheduler());
        }

        public virtual IObservable<Row<TValue>> Query<TValue>(Query query)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            return Observable.Create<Row<TValue>>(async o =>
            {
                var response = await Client.Views.QueryAsync<TValue>(query.ToRequest()).ForAwait();

                ThrowIfNotSuccessfulResponse(response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row<TValue>(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            }).SubscribeOn(ObservableSubscribeOnScheduler());
        }

        public virtual IObservable<Row<TValue, TIncludedDoc>> Query<TValue, TIncludedDoc>(Query query)
        {
            ThrowIfDisposed();

            Ensure.That(query, "query").IsNotNull();

            return Observable.Create<Row<TValue, TIncludedDoc>>(async o =>
            {
                var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query.ToRequest()).ForAwait();

                ThrowIfNotSuccessfulResponse(response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row<TValue, TIncludedDoc>(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            }).SubscribeOn(ObservableSubscribeOnScheduler());
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
    }

    public class QueryInfo
    {
        public long TotalRows { get; private set; }
        public string UpdateSeq { get; private set; }
        public long RowCount { get; private set; }
        public long OffSet { get; private set; }

        public QueryInfo(long totalRows, long rowCount, long offSet, string updateSeq)
        {
            TotalRows = totalRows;
            RowCount = rowCount;
            OffSet = offSet;
            UpdateSeq = updateSeq;
        }
    }
}