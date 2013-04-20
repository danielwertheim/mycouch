namespace MyCouch.Schemes
{
    public interface IEntityAccessor 
    {
        IEntityMember IdMember { get; set; }
        IEntityMember RevMember { get; set; }
    }
}