namespace MyCouch.Rich.EntitySchemes
{
    /// <summary>
    /// Used to get and set specific members of entities when you are using the
    /// typed API.
    /// </summary>
    public interface IEntityReflector 
    {
        IEntityMember IdMember { get; set; }
        IEntityMember RevMember { get; set; }
    }
}