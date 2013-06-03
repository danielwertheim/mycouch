using System.Threading.Tasks;
using MyCouch.Commands;

namespace MyCouch
{
    /// <summary>
    /// Used to access and manage documents. If you want to work with entities, POCOs,
    /// use <see cref="IEntities"/> instead, via <see cref="IClient.Entities"/>.
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
        /// Copies the document having a document id matching <paramref name="srcId"/> to a new document
        /// with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="Copy(CopyDocumentCommand)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        DocumentHeaderResponse Copy(string srcId, string newId);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> to a new document
        /// with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="CopyAsync(CopyDocumentCommand)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(string srcId, string newId);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> and rev matching <paramref name="srcRev"/>
        /// to a new document with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="Copy(CopyDocumentCommand)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        DocumentHeaderResponse Copy(string srcId, string srcRev, string newId);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> and rev matching <paramref name="srcRev"/>
        /// to a new document with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="CopyAsync(CopyDocumentCommand)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(string srcId, string srcRev, string newId);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="cmd"/>.SrcId to a new document
        /// with a new id being <paramref name="cmd"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="cmd"/>.SrcRev
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentHeaderResponse Copy(CopyDocumentCommand cmd);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="cmd"/>.SrcId to a new document
        /// with a new id being <paramref name="cmd"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="cmd"/>.SrcRev.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(CopyDocumentCommand cmd);

        /// <summary>
        /// Replaces the document having a document id matching <paramref name="trgId"/> and rev <paramref name="trgRev"/>
        /// with the document having id matching <paramref name="srcId"/>.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <returns></returns>
        DocumentHeaderResponse Replace(string srcId, string trgId, string trgRev);

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
        DocumentHeaderResponse Replace(string srcId, string srcRev, string trgId, string trgRev);

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
        /// Replaces the document having a document id matching <paramref name="cmd"/>.TrgId and rev <paramref name="cmd"/>.TrgRev
        /// with the document having id matching <paramref name="cmd"/>.SrcId and optional rev <paramref name="cmd"/>.SrcRev.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentHeaderResponse Replace(ReplaceDocumentCommand cmd);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="cmd"/>.SrcId to a new document
        /// with a new id being <paramref name="cmd"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="cmd"/>.SrcRev.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentCommand cmd);

        /// <summary>
        /// Makes a simple HEAD request which doesn not include the actual JSON document,
        /// and returns any matched info for the <paramref name="id"/> and the optional
        /// <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev">optional</param>
        /// <returns></returns>
        DocumentHeaderResponse Exists(string id, string rev = null);

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
        /// and returns any matched info for the <paramref name="cmd"/>.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentHeaderResponse Exists(DocumentExistsCommand cmd);

        /// <summary>
        /// Makes a simple HEAD request which doesn not include the actual JSON document,
        /// and returns any matched info for the <paramref name="cmd"/>.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ExistsAsync(DocumentExistsCommand cmd);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="id">The Id of the document.</param>
        /// <param name="rev">
        /// Optional. Lets you specify a specific document revision.
        /// If not specified, you will get the latest document.
        /// </param>
        /// <returns>Untyped response with JSON.</returns>
        DocumentResponse Get(string id, string rev = null);

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
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentResponse Get(GetDocumentCommand cmd);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentResponse> GetAsync(GetDocumentCommand cmd);

        /// <summary>
        /// Inserts sent JSON document as it is. No additional metadata like doctype will be added.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        DocumentHeaderResponse Post(string doc);

        /// <summary>
        /// Inserts sent JSON document as it is. No additional metadata like doctype will be added.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PostAsync(string doc);

        /// <summary>
        /// Inserts or Updates. The document <paramref name="doc"/> needs to contain the _id and for updates also the _rev field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        DocumentHeaderResponse Put(string id, string doc);

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
        DocumentHeaderResponse Put(string id, string rev, string doc);

        /// <summary>
        /// Inserts or Updates. The document <paramref name="doc"/> needs to contain the _id but not the _rev, neither for inserts nor for updates.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(string id, string rev, string doc);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="id"/> and <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        DocumentHeaderResponse Delete(string id, string rev);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="id"/> and <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(string id, string rev);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="cmd"/>.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentHeaderResponse Delete(DeleteDocumentCommand cmd);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="cmd"/>.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentCommand cmd);
    }
}