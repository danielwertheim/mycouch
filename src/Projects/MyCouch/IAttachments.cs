using System.Threading.Tasks;
using MyCouch.Commands;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to access and manage attachments to documents.
    /// </summary>
    public interface IAttachments
    {
        /// <summary>
        /// Returns only the requested attachment and not the complete document.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="attachmentName"></param>
        /// <returns></returns>
        Task<AttachmentResponse> GetAsync(string docId, string attachmentName);

        /// <summary>
        /// Returns only the requested attachment and not the complete document.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="docRev"></param>
        /// <param name="attachmentName"></param>
        /// <returns></returns>
        Task<AttachmentResponse> GetAsync(string docId, string docRev, string attachmentName);

        /// <summary>
        /// Returns only the requested attachment and not the complete document.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<AttachmentResponse> GetAsync(GetAttachmentCommand cmd);

        /// <summary>
        /// Used to add an attachment to an existing document.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(PutAttachmentCommand cmd);

        /// <summary>
        /// Used to delete an existing attachment.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="docRev"></param>
        /// <param name="attachmentName"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(string docId, string docRev, string attachmentName);

        /// <summary>
        /// Used to delete an existing attachment.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(DeleteAttachmentCommand cmd);
    }
}