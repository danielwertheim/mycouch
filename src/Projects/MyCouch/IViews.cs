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
        Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class;

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
        Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class;
    }
}