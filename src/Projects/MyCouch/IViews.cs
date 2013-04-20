using System;
using System.Threading.Tasks;

namespace MyCouch
{
    /// <summary>
    /// Used to query and manage views.
    /// </summary>
    public interface IViews
    {
        /// <summary>
        /// Creates a query which you later can run against any number of database instaces
        /// using <see cref="RunQuery"/> or <see cref="RunQueryAsync"/>. The query is not
        /// tied to the current connected client.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <returns></returns>
        IViewQuery CreateQuery(string designDocument, string viewname);

        /// <summary>
        /// Lets you run an <see cref="IViewQuery"/> against the current database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ViewQueryResponse RunQuery(IViewQuery query);

        /// <summary>
        /// Lets you run an <see cref="IViewQuery"/> against the current database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ViewQueryResponse<T> RunQuery<T>(IViewQuery query) where T : class;

        /// <summary>
        /// Lets you run an <see cref="IViewQuery"/> against the current database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse> RunQueryAsync(IViewQuery query);

        /// <summary>
        /// Lets you run an <see cref="IViewQuery"/> against the current database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class;

        /// <summary>
        /// Creates and executes an <see cref="IViewQuery"/> on the fly.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        ViewQueryResponse Query(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator);

        /// <summary>
        /// Creates and executes an <see cref="IViewQuery"/> on the fly.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        ViewQueryResponse<T> Query<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class;
        
        /// <summary>
        /// Creates and executes an <see cref="IViewQuery"/> on the fly.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<ViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator);

        /// <summary>
        /// Creates and executes an <see cref="IViewQuery"/> on the fly.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class;
    }
}