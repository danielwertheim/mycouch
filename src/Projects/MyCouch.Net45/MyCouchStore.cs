using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Concurrency;
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

        public Func<IScheduler> ObservableSubscribeOnScheduler { protected get; set; }

        public MyCouchStore(string dbUri) : this(new MyCouchClient(dbUri)) { }

        public MyCouchStore(Uri dbUri) : this(new MyCouchClient(dbUri)) { }

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

        public virtual IObservable<Row> ObserveQuery(Query query)
        {
            ThrowIfDisposed();

            return Observable.Create<Row>(async o =>
            {
                var response = await Client.Views.QueryAsync(query).ForAwait();

                ThrowIfNotSuccessfulResponse(response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            }).SubscribeOn(ObservableSubscribeOnScheduler());
        }

        public virtual IObservable<Row<TValue>> ObserveQuery<TValue>(Query query)
        {
            ThrowIfDisposed();

            return Observable.Create<Row<TValue>>(async o =>
            {
                var response = await Client.Views.QueryAsync<TValue>(query).ForAwait();

                ThrowIfNotSuccessfulResponse(response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row<TValue>(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            }).SubscribeOn(ObservableSubscribeOnScheduler());
        }

        public virtual IObservable<Row<TValue, TIncludedDoc>> ObserveQuery<TValue, TIncludedDoc>(Query query)
        {
            ThrowIfDisposed();

            return Observable.Create<Row<TValue, TIncludedDoc>>(async o =>
            {
                var response = await Client.Views.QueryAsync<TValue, TIncludedDoc>(query).ForAwait();

                ThrowIfNotSuccessfulResponse(response);

                foreach (var row in response.Rows)
                    o.OnNext(new Row<TValue, TIncludedDoc>(row.Id, row.Key, row.Value, row.IncludedDoc));

                o.OnCompleted();

                return Disposable.Empty;
            }).SubscribeOn(ObservableSubscribeOnScheduler());
        }

        protected virtual void ThrowIfNotSuccessfulResponse(Response response)
        {
            if (response.IsSuccess)
                return;

            throw new MyCouchException(response.RequestMethod, response.StatusCode, response.RequestUri, response.Error, response.Reason);
        }
    }

    public static class ObservableRowExtensions
    {
        public static List<Row> ToListOfRows(this IObservable<Row> ob)
        {
            var result = new List<Row>();

            ob.ForEachAsync((row, i) => result.Add(row));

            return result;
        }

        public static List<Row<TValue>> ToListOfRows<TValue>(this IObservable<Row<TValue>> ob)
        {
            var result = new List<Row<TValue>>();

            ob.ForEachAsync((row, i) => result.Add(row));

            return result;
        }

        public static List<Row<TValue, TIncludedDoc>> ToListOfRows<TValue, TIncludedDoc>(this IObservable<Row<TValue, TIncludedDoc>> ob)
        {
            var result = new List<Row<TValue, TIncludedDoc>>();

            ob.ForEachAsync((row, i) => result.Add(row));

            return result;
        }
    }
}