using System;
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
        IMyCouchClient Client { get; }
        Task CopyAsync(string srcId, string newId);
        Task CopyAsync(string srcId, string srcRev, string newId);
        Task ReplaceAsync(string srcId, string trgId, string trgRev);
        Task ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev);
        Task DeleteAsync(string id, string rev);
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<bool> ExistsAsync(string id, string rev = null);
        Task<DocumentHeader> GetHeaderAsync(string id, string rev = null);
        Task<string> GetByIdAsync(string id, string rev = null);
        Task<TEntity> GetByIdAsync<TEntity>(string id, string rev = null) where TEntity : class;
        IObservable<Row> Query(Query query);
        IObservable<Row<TValue>> Query<TValue>(Query query);
        IObservable<Row<TValue, TIncludedDoc>> Query<TValue, TIncludedDoc>(Query query);
    }
}