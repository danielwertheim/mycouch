using MyCouch.EntitySchemes.Reflections;

namespace MyCouch.EntitySchemes
{
    public class EntityReflector : IEntityReflector
    {
        public IEntityMember IdMember { get; protected set; }
        public IEntityMember RevMember { get; protected set; }

        public EntityReflector(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            IdMember = new EntityIdMember(dynamicPropertyFactory);
            RevMember = new EntityRevMember(dynamicPropertyFactory);
        }
    }
}