namespace MyCouch.Schemes
{
    public class EntityAccessor : IEntityAccessor
    {
        public IEntityMember IdMember { get; set; }
        public IEntityMember RevMember { get; set; }

        public EntityAccessor()
        {
            IdMember = new EntityIdMember();
            RevMember = new EntityRevMember();
        }
    }
}