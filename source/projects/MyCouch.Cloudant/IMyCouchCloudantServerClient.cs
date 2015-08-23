namespace MyCouch.Cloudant
{
    /// <summary>
    /// Connects to a server instance rather than a DB instance
    /// as <see cref="IMyCouchCloudantClient"/> or <see cref="IMyCouchStore"/>
    /// does.
    /// </summary>
    public interface IMyCouchCloudantServerClient : IMyCouchServerClient
    {
        /// <summary>
        /// Used to access Security features with Cloudant.
        /// </summary>
        ISecurity Security { get; }
    }
}