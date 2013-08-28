using System;
using System.Threading.Tasks;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to query and manage views.
    /// </summary>
    public interface IViews
    {
        /// <summary>
        /// Lets you run an <see cref="IViewQuery"/>.
        /// The resulting <see cref="JsonViewQueryResponse"/> will consist of
        /// Rows being JSON-strings.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<JsonViewQueryResponse> RunQueryAsync(IViewQuery query);

        /// <summary>
        /// Lets you run an <see cref="IViewQuery"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class;

        /// <summary>
        /// Creates and executes an <see cref="IViewQuery"/> on the fly.
        /// The resulting <see cref="JsonViewQueryResponse"/> will consist of
        /// Rows being JSON-strings.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<JsonViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator);

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