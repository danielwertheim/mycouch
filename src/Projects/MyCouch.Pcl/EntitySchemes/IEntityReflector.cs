namespace MyCouch.EntitySchemes
{
    public interface IEntityReflector
    {
        IEntityMember IdMember { get; }
        IEntityMember RevMember { get; }
    }
}