using System.Threading.Tasks;

namespace MyCouch
{
    /// <summary>
    /// Used to access and manage documents.
    /// </summary>
    public interface IDocuments
    {
        /// <summary>
        /// Lets you use the underlying bulk API to insert, update and delete
        /// documents.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        BulkResponse Bulk(BulkCommand cmd);

        /// <summary>
        /// Lets you use the underlying bulk API to insert, update and delete
        /// documents.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<BulkResponse> BulkAsync(BulkCommand cmd);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="id">The Id of the document.</param>
        /// <param name="rev">
        /// Optional. Lets you specify a specific document revision.
        /// If not specified, you will get the latest document.
        /// </param>
        /// <returns>Untyped response with JSON.</returns>
        JsonDocumentResponse Get(string id, string rev = null);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="id">The Id of the document.</param>
        /// <param name="rev">
        /// Optional. Lets you specify a specific document revision.
        /// If not specified, you will get the latest document.
        /// </param>
        /// <returns>Untyped response with JSON.</returns>
        Task<JsonDocumentResponse> GetAsync(string id, string rev = null);

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
        EntityResponse<T> Get<T>(string id, string rev = null) where T : class;

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
        /// Inserts sent JSON document as it is. No additional metadata like doctype will be added.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        JsonDocumentResponse Post(string doc);

        /// <summary>
        /// Inserts sent JSON document as it is. No additional metadata like doctype will be added.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<JsonDocumentResponse> PostAsync(string doc);

        /// <summary>
        /// Inserts sent entity. The resulting JSON that is inserted will have some additional
        /// meta-data contained in the JSON, like doctype.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityResponse<T> Post<T>(T entity) where T : class;

        /// <summary>
        /// Inserts sent entity. The resulting JSON that is inserted will have some additional
        /// meta-data contained in the JSON, like doctype.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PostAsync<T>(T entity) where T : class;

        /// <summary>
        /// Updates or Inserts entity. The document <paramref name="doc"/> needs to contain the _rev field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        JsonDocumentResponse Put(string id, string doc);

        /// <summary>
        /// Updates or Inserts entity. The document <paramref name="doc"/> needs to contain the _rev field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<JsonDocumentResponse> PutAsync(string id, string doc);

        /// <summary>
        /// Updates entity, without having to specify _rev field in the document <paramref name="doc"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        JsonDocumentResponse Put(string id, string rev, string doc);

        /// <summary>
        /// Updates entity, without having to specify _rev field in the document <paramref name="doc"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<JsonDocumentResponse> PutAsync(string id, string rev, string doc);

        /// <summary>
        /// Updates sent entity and returns it in the response, and if successful, then with and
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityResponse<T> Put<T>(T entity) where T : class;

        /// <summary>
        /// Updates sent entity and returns it in the response, and if successful, then with and
        /// updated _rev value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> PutAsync<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the document that matches sent <paramref name="id"/> and <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        JsonDocumentResponse Delete(string id, string rev);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="id"/> and <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<JsonDocumentResponse> DeleteAsync(string id, string rev);

        /// <summary>
        /// Deletes the document that matches the values of the document _id and _rev extracted from <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityResponse<T> Delete<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the document that matches the values of the document _id and _rev extracted from <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityResponse<T>> DeleteAsync<T>(T entity) where T : class;
    }
}