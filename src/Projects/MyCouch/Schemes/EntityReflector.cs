namespace MyCouch.Schemes
{
    public class EntityReflector : IEntityReflector
    {
        public IEntityMember IdMember { get; set; }
        public IEntityMember RevMember { get; set; }

        public EntityReflector()
        {
            IdMember = new EntityIdMember();
            RevMember = new EntityRevMember();
        }
    }
}