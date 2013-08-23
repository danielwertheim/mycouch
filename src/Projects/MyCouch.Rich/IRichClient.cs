namespace MyCouch.Rich
{
    public interface IRichClient : IClient
    {
        /// <summary>
        /// Entity oriented API operations, for accessing and managing documents as entities.
        /// </summary>
        IEntities Entities { get; }
    }
}