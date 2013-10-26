using System;
using System.Reflection;
using MyCouch.EntitySchemes.Reflections;

namespace MyCouch.EntitySchemes
{
    public class EntityRevMember : EntityMember 
    {
        public EntityRevMember(IDynamicPropertyFactory dynamicPropertyFactory) 
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

            if (membername.Equals("_rev", StringComparison.OrdinalIgnoreCase))
                return 0 * factor;

            if (membername.Equals(string.Concat(entityType.Name, "rev"), StringComparison.OrdinalIgnoreCase))
                return 1 * factor;

            if (membername.Equals("documentrev", StringComparison.OrdinalIgnoreCase))
                return 2 * factor;

            if (membername.Equals("entityrev", StringComparison.OrdinalIgnoreCase))
                return 3 * factor;

            if (membername.Equals("rev", StringComparison.OrdinalIgnoreCase))
                return 4 * factor;
#if !NETFX_CORE
            return entityType.BaseType == typeof(object)
                ? null
                : GetMemberRankingIndex(entityType.BaseType, membername, (factor * 10));
#else
            var typeInfo = entityType.GetTypeInfo();
            return typeInfo.BaseType == typeof(object)
                ? null
                : GetMemberRankingIndex(typeInfo.BaseType, membername, (factor * 10));
#endif
        }
    }
}