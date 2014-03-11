using System;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses;

namespace MyCouch
{
    public class MyCouchStore : IMyCouchStore
    {
        protected bool IsDisposed { get; private set; }

        public IMyCouchClient Client { get; protected set; }

        public MyCouchStore(string dbUri) : this(new MyCouchClient(dbUri)) { }

        public MyCouchStore(Uri dbUri) : this(new MyCouchClient(dbUri)) { }

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
        }

        protected virtual void Dispose(bool disposing)
        {
            ThrowIfDisposed();

            IsDisposed = true;

            if (!disposing)
                return;

            Client.Dispose();
            Client = null;
        }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public virtual async Task CopyAsync(string srcId, string newId)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.CopyAsync(srcId, newId).ForAwait();

            ThrowIfNotSuccessfulResponse("CopyAsync", response);
        }

        public virtual async Task CopyAsync(string srcId, string srcRev, string newId)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.CopyAsync(srcId, srcRev, newId).ForAwait();

            ThrowIfNotSuccessfulResponse("CopyAsync", response);
        }

        public virtual async Task ReplaceAsync(string srcId, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.ReplaceAsync(srcId, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse("ReplaceAsync", response);
        }

        public virtual async Task ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.ReplaceAsync(srcId, srcRev, trgId, trgRev).ForAwait();

            ThrowIfNotSuccessfulResponse("ReplaceAsync", response);
        }

        public virtual async Task DeleteAsync(string id, string rev)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.DeleteAsync(id, rev).ForAwait();

            ThrowIfNotSuccessfulResponse("DeleteAsync", response, HttpStatusCode.NotFound);
        }

        public virtual async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            ThrowIfDisposed();

            var response = await Client.Entities.DeleteAsync(entity).ForAwait();

            ThrowIfNotSuccessfulResponse("DeleteAsync<TEntity>", response, HttpStatusCode.NotFound);
        }

        public virtual async Task<bool> ExistsAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            ThrowIfNotSuccessfulResponse("ExistsAsync", response);

            return response.IsSuccess;
        }

        public virtual async Task<DocumentHeader> GetHeaderAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            ThrowIfNotSuccessfulResponse("GetHeaderAsync", response, HttpStatusCode.NotFound);

            return new DocumentHeader(response.Id, response.Rev);
        }

        public virtual async Task<string> GetByIdAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.GetAsync(id, rev).ForAwait();

            ThrowIfNotSuccessfulResponse("GetByIdAsync", response, HttpStatusCode.NotFound);

            return response.Content;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class
        {
            ThrowIfDisposed();

            var response = await Client.Entities.GetAsync<TEntity>(id, rev);

            ThrowIfNotSuccessfulResponse("GetByIdAsync<TEntity>", response, HttpStatusCode.NotFound);

            return response.Content;
        }

        public virtual IObservable<Row> Query(Query query)
        {
            ThrowIfDisposed();

            return Observable.Create<Row>(async o =>
            {
                var response = await Client.Views.QueryAsync(query);

                ThrowIfNotSuccessfulResponse("Query", response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            });
        }

        public virtual IObservable<Row<TValue>> Query<TValue>(Query query)
        {
            ThrowIfDisposed();

            return Observable.Create<Row<TValue>>(async o =>
            {
                var response = await Client.Views.QueryAsync<TValue>(query);

                ThrowIfNotSuccessfulResponse("Query", response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row<TValue>(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            });
        }

        public virtual IObservable<Row<TValue, TIncludedDoc>> Query<TValue, TIncludedDoc>(Query query)
        {
            ThrowIfDisposed();

            return Observable.Create<Row<TValue, TIncludedDoc>>(async o =>
            {
                var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query);

                ThrowIfNotSuccessfulResponse("Query", response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row<TValue, TIncludedDoc>(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            });
        }

        protected virtual void ThrowIfNotSuccessfulResponse(string action, Response response, params HttpStatusCode[] allowedFailedStatusCodes)
        {
            if (response.IsSuccess)
                return;

            if (allowedFailedStatusCodes != null && allowedFailedStatusCodes.Contains(response.StatusCode))
                return;

            throw new MyCouchException(string.Format(
                "Exception while performing '{1}'{0}Status: {2}{0}Error: {3}{0}Reason: {4}{0}",
                Environment.NewLine,
                action,
                response.StatusCode,
                response.Error,
                response.Reason));
        }
    }
}