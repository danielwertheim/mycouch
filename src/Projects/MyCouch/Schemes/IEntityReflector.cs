namespace MyCouch.Schemes
{
    public interface IEntityReflector 
    {
        IEntityMember IdMember { get; set; }
        IEntityMember RevMember { get; set; }
    }
}