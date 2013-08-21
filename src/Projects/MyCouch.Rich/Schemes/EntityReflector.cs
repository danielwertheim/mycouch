using MyCouch.Schemes.Reflections;

namespace MyCouch.Schemes
{
    public class EntityReflector : IEntityReflector
    {
        public IEntityMember IdMember { get; set; }
        public IEntityMember RevMember { get; set; }

        public EntityReflector(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            IdMember = new EntityIdMember(dynamicPropertyFactory);
            RevMember = new EntityRevMember(dynamicPropertyFactory);
        }
    }
}