using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Serialization;

namespace MyCouch
{
    /// <summary>
    /// Used to access and manage documents. If you want to work with entities, POCOs,
    /// use <see cref="IEntities"/> instead, via <see cref="IMyCouchClient.Entities"/>.
    /// </summary>
    public interface IDocuments
    {
        /// <summary>
        /// The Serializer associated with this <see cref="IDocuments"/> instance. Use this if you want
        /// to serialize or deserialize using document conventions.
        /// </summary>
        /// <remarks>
        /// If you want the native, non convention based serializer, check <see cref="IMyCouchClient.Serializer"/>
        /// If you want full entity convention based serializer, check <see cref="IEntities.Serializer"/>
        /// </remarks>
        IDocumentSerializer Serializer { get; }

        /// <summary>
        /// Lets you use the underlying bulk API to insert, update and delete
        /// documents.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BulkResponse> BulkAsync(BulkRequest request);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> to a new document
        /// with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="CopyAsync(CopyDocumentRequest)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(string srcId, string newId);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> and rev matching <paramref name="srcRev"/>
        /// to a new document with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="CopyAsync(CopyDocumentRequest)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(string srcId, string srcRev, string newId);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="request"/>.SrcId to a new document
        /// with a new id being <paramref name="request"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="request"/>.SrcRev.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(CopyDocumentRequest request);

        /// <summary>
        /// Replaces the document having a document id matching <paramref name="trgId"/> and rev <paramref name="trgRev"/>
        /// with the document having id matching <paramref name="srcId"/>.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string trgId, string trgRev);

        /// <summary>
        /// Replaces the document having a document id matching <paramref name="trgId"/> and rev <paramref name="trgRev"/>
        /// with the document having id matching <paramref name="srcId"/> and rev matching <paramref name="srcRev"/>.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="request"/>.SrcId to a new document
        /// with a new id being <paramref name="request"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="request"/>.SrcRev.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentRequest request);

        /// <summary>
        /// Makes a simple HEAD request which doesn not include the actual JSON document,
        /// and returns any matched info for the <paramref name="id"/> and the optional
        /// <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev">optional</param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ExistsAsync(string id, string rev = null);

        /// <summary>
        /// Makes a simple HEAD request which doesn not include the actual JSON document,
        /// and returns any matched info for the <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ExistsAsync(DocumentExistsRequest request);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="id">The Id of the document.</param>
        /// <param name="rev">
        /// Optional. Lets you specify a specific document revision.
        /// If not specified, you will get the latest document.
        /// </param>
        /// <returns>Untyped response with JSON.</returns>
        Task<DocumentResponse> GetAsync(string id, string rev = null);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentResponse> GetAsync(GetDocumentRequest request);

        /// <summary>
        /// Inserts sent JSON document as it is. No additional metadata like doctype will be added.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PostAsync(string doc);

        /// <summary>
        /// Inserts sent JSON document as it is. No additional metadata like doctype will be added.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PostAsync(PostDocumentRequest request);

        /// <summary>
        /// Inserts or Updates. The document <paramref name="doc"/> needs to contain the _id and for updates also the _rev field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(string id, string doc);

        /// <summary>
        /// Inserts or Updates. The document <paramref name="doc"/> needs to contain the _id but not the _rev, neither for inserts nor for updates.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(string id, string rev, string doc);

        /// <summary>
        /// Inserts or Updates. The document in passed <paramref name="request"/>.
        /// The document needs to contain the _id but not the _rev, neither for inserts nor for updates.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(PutDocumentRequest request);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="id"/> and <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(string id, string rev);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentRequest request);
    }
}