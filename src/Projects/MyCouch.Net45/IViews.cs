using System;
using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Requests.Configurators;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to query views.
    /// </summary>
    public interface IViews
    {
        /// <summary>
        /// Lets you perform a query by using a reusable <see cref="QueryViewRequest"/>.
        /// Any returned Value and, or IncludedDoc of the response,
        /// will be treated as JSON-strings.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse> QueryAsync(QueryViewRequest query);

        /// <summary>
        /// Lets you perform a query by using a reusable <see cref="QueryViewRequest"/>.
        /// Any returned Value of the response,
        /// will be treated as defined by <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<TValue>> QueryAsync<TValue>(QueryViewRequest query);

        /// <summary>
        /// Lets you perform a query by using a reusable <see cref="QueryViewRequest"/>.
        /// Any returned Value of the response,
        /// will be treated as defined by <typeparamref name="TValue"/>.
        /// Any returned IncludedDoc of the response,
        /// will be treated as defined by <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TIncludedDoc"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<TValue, TIncludedDoc>> QueryAsync<TValue, TIncludedDoc>(QueryViewRequest query);

        /// <summary>
        /// Creates and executes an <see cref="QueryViewRequest"/> on the fly.
        /// Any returned Value and, or IncludedDoc of the response,
        /// will be treated as JSON-strings.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<ViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<QueryViewRequestConfigurator> configurator);

        /// <summary>
        /// Creates and executes an <see cref="QueryViewRequest"/> on the fly.
        /// Any returned Value of the response,
        /// will be treated as defined by <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<TValue>> QueryAsync<TValue>(string designDocument, string viewname, Action<QueryViewRequestConfigurator> configurator);

        /// <summary>
        /// Creates and executes an <see cref="QueryViewRequest"/> on the fly.
        /// Any returned Value of the response,
        /// will be treated as defined by <typeparamref name="TValue"/>.
        /// Any returned IncludedDoc of the response,
        /// will be treated as defined by <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TIncludedDoc"></typeparam>
        /// <param name="designDocument"></param>
        /// <param name="viewname"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<TValue, TIncludedDoc>> QueryAsync<TValue, TIncludedDoc>(string designDocument, string viewname, Action<QueryViewRequestConfigurator> configurator);
    }
}