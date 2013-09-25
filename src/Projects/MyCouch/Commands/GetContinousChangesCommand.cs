using System;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class GetContinousChangesCommand
    {
        /// <summary>
        /// Set a millisecond value to have CouchDbReport to send a
        /// newline at every tick where the length between the ticks
        /// is the value you define.
        /// </summary>
        public int? HeartBeatMs { get; set; }
        /// <summary>
        /// Determines if the response should include the docs
        /// that are affected by the change(s).
        /// </summary>
        public bool? IncludeDocs { get; set; }

        public virtual GetContinousChangesCommand SetHeartBeatMs(int? value)
        {
            HeartBeatMs = value;

            return this;
        }
    }
}