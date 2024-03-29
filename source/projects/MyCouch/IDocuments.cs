﻿using System.Threading;
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
        ISerializer Serializer { get; }

        /// <summary>
        /// Lets you use the underlying bulk API to insert, update and delete
        /// documents.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<BulkResponse> BulkAsync(BulkRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets you use the underlying purge API to permanently remove the references to documents in the database
        /// documents.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PurgeResponse> PurgeAsync(PurgeRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> to a new document
        /// with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="CopyAsync(CopyDocumentRequest, CancellationToken)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="newId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(string srcId, string newId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="srcId"/> and rev matching <paramref name="srcRev"/>
        /// to a new document with a new id being <paramref name="newId"/>.
        /// For more options use <see cref="CopyAsync(CopyDocumentRequest, CancellationToken)"/> instead.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="newId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(string srcId, string srcRev, string newId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="request"/>.SrcId to a new document
        /// with a new id being <paramref name="request"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="request"/>.SrcRev.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> CopyAsync(CopyDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the document having a document id matching <paramref name="trgId"/> and rev <paramref name="trgRev"/>
        /// with the document having id matching <paramref name="srcId"/>.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string trgId, string trgRev, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the document having a document id matching <paramref name="trgId"/> and rev <paramref name="trgRev"/>
        /// with the document having id matching <paramref name="srcId"/> and rev matching <paramref name="srcRev"/>.
        /// </summary>
        /// <param name="srcId"></param>
        /// <param name="srcRev"></param>
        /// <param name="trgId"></param>
        /// <param name="trgRev"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copies the document having a document id matching <paramref name="request"/>.SrcId to a new document
        /// with a new id being <paramref name="request"/>.NewId.
        /// You can also specify a specific revision to copy via <paramref name="request"/>.SrcRev.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Makes a simple HEAD request which does not include the actual JSON document,
        /// and returns any matched info for the <paramref name="id"/> and the optional
        /// <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev">optional</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> HeadAsync(string id, string rev = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Makes a simple HEAD request which doesn not include the actual JSON document,
        /// and returns any matched info for the <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> HeadAsync(HeadDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="id">The Id of the document.</param>
        /// <param name="rev">
        /// Optional. Lets you specify a specific document revision.
        /// If not specified, you will get the latest document.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>Untyped response with JSON.</returns>
        Task<DocumentResponse> GetAsync(string id, string rev = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets untyped response with the JSON representation of the document.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentResponse> GetAsync(GetDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts sent JSON document as it is.
        /// Underlying DB will generate _id if non is provided in <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PostAsync(string doc, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts sent JSON document as it is.
        /// Underlying DB will generate _id if non is provided in Content of <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PostAsync(PostDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts or Updates.
        /// For updates, <paramref name="doc"/> needs the _rev field.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(string id, string doc, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts or Updates.
        /// If _id in <paramref name="doc"/> is different than the one specified in
        /// <paramref name="doc"/>, the one in <paramref name="id"/> will be used.
        /// If _rev in <paramref name="rev"/> is different than the one specified in
        /// <paramref name="doc"/>, the one in <paramref name="rev"/> will be used.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="doc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(string id, string rev, string doc, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts or Updates.
        /// If _id in Content of <paramref name="request"/> is different than the one specified in
        /// Id of <paramref name="request"/>, the one in Id of <paramref name="request"/> will be used.
        /// If _rev in Content of <paramref name="request"/> is different than the one specified in
        /// Rev of <paramref name="request"/>, the one in Rev of <paramref name="request"/> will be used.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(PutDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="id"/> and <paramref name="rev"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rev"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(string id, string rev, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the document that matches sent <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Transforms a document with a show function
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RawResponse> ShowAsync(QueryShowRequest request, CancellationToken cancellationToken = default);
    }
}