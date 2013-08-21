using System.Threading.Tasks;
using MyCouch.Commands;

namespace MyCouch.Rich
{
    /// <summary>
    /// Used to access and manage documents as entities.
    /// </summary>
    public interface IEntities
    {
        /// <summary>
        /// Gets typed entity-response (<see cref="EntityResponse{T}"/> of <typeparamref name="T"/>)
        /// representation of the document.
        /// </summary>
        /// <typeparam name="T">The type you want the document deserialized as.</typeparam>
        /// <param name="id">The Id of the document.</param>
        /// <param name="rev">
        /// Optional. Lets you specify a specific document revision.
        /// If not specified, you will get the latest document.
        /// </param>
        /// <returns>
        /// Typed entity-response (<see cref="EntityResponse{T}"/> of <typeparamref name="T"/>)
        /// representation of the document
        /// </returns>
        Task<EntityResponse<T>> GetAsync<T>(string id, string rev = null) where T : class;

        /// <summary>
        /// Gets typed entity-response (<see cref="EntityResponse{T}"/> of <typeparamref name="T"/>)
        /// representation of the document.
        /// </summary>
        /// <typeparam name="T">The type you want the document deserialized as.</typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> GetAsync<T>(GetEntityCommand cmd) where T : class;

        /// <summary>
        /// Inserts sent entity. The resulting JSON that is inserted will have some additional
        /// meta-data contained in the JSON, like $doctype.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class;

        /// <summary>
        /// Inserts sent entity. The resulting JSON that is inserted will have some additional
        /// meta-data contained in the JSON, like $doctype.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PostAsync<T>(PostEntityCommand<T> cmd) where T : class;

        /// <summary>
        /// Inserts or Updates sent entity and returns it in the response, and if successful, then with an
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class;

        /// <summary>
        /// Inserts or Updates sent entity and returns it in the response, and if successful, then with an
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(PutEntityCommand<T> cmd) where T : class;

        /// <summary>
        /// Deletes the document that matches the values of the document _id and _rev extracted from <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class;
    }
}