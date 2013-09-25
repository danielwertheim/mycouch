using System;
using System.Threading.Tasks;
using MyCouch.Commands;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to consume the changes feed.
    /// </summary>
    public interface IChanges
    {
        Task<ChangesResponse> GetContinuouslyAsync(Action<GetContinousChangesCommand> cfg = null);
        Task<ChangesResponse> GetAsync(GetContinousChangesCommand cmd);
    }
}