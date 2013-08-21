using System;
using MyCouch.Rich.Schemes.Reflections;

namespace MyCouch.Rich.Schemes
{
    public class EntityIdMember : EntityMember
    {
        public EntityIdMember(IDynamicPropertyFactory dynamicPropertyFactory)
            : base(dynamicPropertyFactory)
        {
        }

        public override int? GetMemberRankingIndex(Type entityType, string membername)
        {
            if (membername.Equals("_id", StringComparison.OrdinalIgnoreCase))
                return 0;

            if (membername.Equals(string.Concat(entityType.Name, "id"), StringComparison.OrdinalIgnoreCase))
                return 1;

            if (membername.Equals("documentid", StringComparison.OrdinalIgnoreCase))
                return 2;

            if (membername.Equals("entityid", StringComparison.OrdinalIgnoreCase))
                return 3;

            if (membername.Equals("id", StringComparison.OrdinalIgnoreCase))
                return 4;

            return null;
        }
    }
}