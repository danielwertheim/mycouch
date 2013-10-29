using System;
using System.Threading.Tasks;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Requests.Configurators;
using MyCouch.Cloudant.Responses;

namespace MyCouch.Cloudant
{
    public interface ISearches
    {
        /// <summary>
        /// Lets you perform a search using Cloudants Lucene powered
        /// Search API by using a reusable <see cref="SearchIndexRequest"/>.
        /// Any returned IncludedDoc will be treated as JSON.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SearchIndexResponse> SearchAsync(SearchIndexRequest request);
        /// <summary>
        /// Lets you perform a search using Cloudants Lucene powered
        /// Search API by using a reusable <see cref="SearchIndexRequest"/>.
        /// Any returned IncludedDoc will be treated as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest request);

        /// <summary>
        /// Lets you perform a search using Cloudants Lucene powered
        /// Search API.
        /// Any returned IncludedDoc will be treated as JSON.
        /// </summary>
        /// <param name="designDocument"></param>
        /// <param name="searchIndexName"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<SearchIndexResponse> SearchAsync(string designDocument, string searchIndexName, Action<SearchIndexRequestConfigurator> configurator);

        /// <summary>
        /// Lets you perform a search using Cloudants Lucene powered
        /// Search API.
        /// Any returned IncludedDoc will be treated as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc"></typeparam>
        /// <param name="designDocument"></param>
        /// <param name="searchIndexName"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(string designDocument, string searchIndexName, Action<SearchIndexRequestConfigurator> configurator);
    }
}