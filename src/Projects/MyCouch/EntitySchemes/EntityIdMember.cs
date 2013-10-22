using System;
using MyCouch.EntitySchemes.Reflections;

namespace MyCouch.EntitySchemes
{
    public class EntityIdMember : EntityMember
    {
        public EntityIdMember(IDynamicPropertyFactory dynamicPropertyFactory)
            : base(dynamicPropertyFactory)
        {
        }

        public override int? GetMemberRankingIndex(Type entityType, string membername)
        {
            return GetMemberRankingIndex(entityType, membername, 0);
        }

        protected virtual int? GetMemberRankingIndex(Type entityType, string membername, int level)
        {
            var factor = level <= 0 ? 1 : level;

            if (membername.Equals("_id", StringComparison.OrdinalIgnoreCase))
                return 0 * factor;

            if (membername.Equals(string.Concat(entityType.Name, "id"), StringComparison.OrdinalIgnoreCase))
                return 1 * factor;

            if (membername.Equals("documentid", StringComparison.OrdinalIgnoreCase))
                return 2 * factor;

            if (membername.Equals("entityid", StringComparison.OrdinalIgnoreCase))
                return 3 * factor;

            if (membername.Equals("id", StringComparison.OrdinalIgnoreCase))
                return 4 * factor;

            return entityType.BaseType == typeof (object) 
                ? null
                : GetMemberRankingIndex(entityType.BaseType, membername, (factor * 10));
        }
    }
}