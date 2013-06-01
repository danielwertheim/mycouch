using System.Threading.Tasks;

namespace MyCouch
{
    /// <summary>
    /// Used to access and manage attachments to documents.
    /// </summary>
    public interface IAttachments
    {
        /// <summary>
        /// Used to add an attachment to an existing document.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentHeaderResponse Put(PutAttachmentCommand cmd);
        
        /// <summary>
        /// Used to add an attachment to an existing document.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> PutAsync(PutAttachmentCommand cmd);

        /// <summary>
        /// Used to delete an existing attachment.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DocumentHeaderResponse Delete(DeleteAttachmentCommand cmd);
        
        /// <summary>
        /// Used to delete an existing attachment.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<DocumentHeaderResponse> DeleteAsync(DeleteAttachmentCommand cmd);
    }
}