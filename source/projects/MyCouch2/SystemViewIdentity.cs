namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain system view like all_docs.
    /// </summary>
    public class SystemViewIdentity : ViewIdentity
    {
        public static SystemViewIdentity AllDocs
        {
            get { return new SystemViewIdentity("_all_docs"); }
        }

        public SystemViewIdentity(string name) : base(name) { }
    }
}