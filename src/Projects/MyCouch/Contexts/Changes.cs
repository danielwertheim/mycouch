using System;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Commands;
using MyCouch.Responses;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        public Changes(IConnection connection) : base(connection) { }

        public Task<ChangesResponse> GetContinuouslyAsync(Action<GetContinousChangesCommand> cfg = null)
        {
            var cmd = new GetContinousChangesCommand();

            if (cfg != null) cfg(cmd);

            return GetAsync(cmd);
        }

        public async Task<ChangesResponse> GetAsync(GetContinousChangesCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            throw new NotImplementedException();
        }
    }
}