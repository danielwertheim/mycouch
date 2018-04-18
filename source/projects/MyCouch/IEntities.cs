using System.Threading;
using System.Threading.Tasks;
using MyCouch.EntitySchemes;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Serialization;

namespace MyCouch
{
    /// <summary>
    /// Used to access and manage documents as entities.
    /// </summary>
    public interface IEntities
    {
        /// <summary>
        /// The Serializer associated with this <see cref="IEntities"/> instance. Use this if you want
        /// to serialize or deserialize using entity conventions.
        /// </summary>
        /// <remarks>If you want the native, non convention based serializer, check <see cref="IMyCouchClient.Serializer"/></remarks>
        ISerializer Serializer { get; }

        /// <summary>
        /// Used to get and set specific members of entities when you are using the
        /// typed API.
        /// </summary>
        IEntityReflector Reflector { get; }

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
        Task<GetEntityResponse<T>> GetAsync<T>(string id, string rev = null, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Gets typed entity-response (<see cref="EntityResponse{T}"/> of <typeparamref name="T"/>)
        /// representation of the document.
        /// </summary>
        /// <typeparam name="T">The type you want the document deserialized as.</typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetEntityResponse<T>> GetAsync<T>(GetEntityRequest request, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Inserts sent entity. The resulting JSON that is inserted will have some additional
        /// meta-data contained in the JSON, like $doctype.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PostAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Inserts sent entity. The resulting JSON that is inserted will have some additional
        /// meta-data contained in the JSON, like $doctype.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PostAsync<T>(PostEntityRequest<T> request, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Inserts (if no _rev is defined) or Updates (if _rev is defined) sent entity and returns it in the response, and if successful, then with an
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Inserts (if no _rev is defined) or Updates (if _rev is defined) sent entity and returns it in the response, and if successful, then with an
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Used as explicit id instead of extracting it from the entity.</param>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(string id, T entity, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Inserts (if no _rev is defined) or Updates (if _rev is defined) sent entity and returns it in the response, and if successful, then with an
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Used as explicit id instead of extracting it from the entity.</param>
        /// <param name="rev">Used as explicit rev instead of extracting it from the entity.</param>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(string id, string rev, T entity, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Inserts or Updates sent entity and returns it in the response, and if successful, then with an
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(PutEntityRequest<T> request, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Deletes the document that matches the values of the document _id and _rev extracted from <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    }
}