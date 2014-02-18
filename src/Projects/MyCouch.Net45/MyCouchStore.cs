using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MyCouch.Extensions;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// A somewhat opinionated abstraction over MyCouch which removes the
    /// use of Http-responses.
    /// If a non successful operation, an exception is thrown.
    /// </summary>
    public class MyCouchStore : IDisposable
    {
        public IMyCouchClient Client { get; protected set; }
        protected bool IsDisposed { get; private set; }

        public MyCouchStore(string dbUri) : this(new MyCouchClient(dbUri)) { }

        public MyCouchStore(Uri dbUri) : this(new MyCouchClient(dbUri)) { }

        public MyCouchStore(IMyCouchClient client)
        {
            Client = client;
            IsDisposed = false;
        }

        public virtual void Dispose()
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

        public virtual async Task<GetHeaderResult> GetHeaderAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.HeadAsync(id, rev).ForAwait();

            ThrowIfNotSuccessfulResponse("GetHeaderAsync", response, HttpStatusCode.NotFound);

            return new GetHeaderResult(response.Id, response.Rev);
        }

        public virtual async Task<string> GetByIdAsync(string id, string rev = null)
        {
            ThrowIfDisposed();

            var response = await Client.Documents.GetAsync(id, rev).ForAwait();

            ThrowIfNotSuccessfulResponse("GetAsync", response, HttpStatusCode.NotFound);

            return response.Content;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class
        {
            ThrowIfDisposed();

            var response = await Client.Entities.GetAsync<TEntity>(id, rev);

            ThrowIfNotSuccessfulResponse("GetAsync<TEntity>", response, HttpStatusCode.NotFound);

            return response.Content;
        }

        //TODO: Look at Task Data Flow lib
        //public virtual async Task<IEnumerable<QueryResult>> QueryAsync(string designDocument, string viewName, Action<QueryViewRequestConfigurator> configurator)
        //{
        //    ThrowIfDisposed();

        //    var response = await Client.Views.QueryAsync(designDocument, viewName, configurator).ForAwait();

        //    ThrowIfNotSuccessfulResponse("QueryAsync", response);

        //    foreach (var row in response.Rows)
        //        yield return new QueryResult(row.Id, row.Key, row.Value, row.IncludedDoc);
        //}

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

    [Serializable]
    public class QueryResult : QueryResult<string, string>
    {
        public QueryResult(string id, object key, string value = null, string includedDoc = null)
            : base(id, key, value, includedDoc)
        {
        }
    }

    [Serializable]
    public class QueryResult<TValue, TIncludedDoc>
        where TValue : class
        where TIncludedDoc : class
    {
        public string Id { get; private set; }
        public object Key { get; private set; }
        public TValue Value { get; private set; }
        public TIncludedDoc IncludedDoc { get; private set; }

        public QueryResult(string id, object key, TValue value = null, TIncludedDoc includedDoc = null)
        {
            Id = id;
            Key = key;
            Value = value;
            IncludedDoc = includedDoc;
        }
    }

    [Serializable]
    public class GetHeaderResult
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public GetHeaderResult(string id, string rev)
        {
            Id = id;
            Rev = rev;
        }
    }
}