using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EnsureThat;
using MyCouch.EntitySchemes.Reflections;

namespace MyCouch.EntitySchemes
{
    public abstract class EntityMember : IEntityMember 
    {
        protected readonly IDynamicPropertyFactory DynamicPropertyFactory;
        protected readonly ConcurrentDictionary<Type, DynamicProperty> IdPropertyCache;

        protected EntityMember(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            Ensure.That(dynamicPropertyFactory, "dynamicPropertyFactory").IsNotNull();

            DynamicPropertyFactory = dynamicPropertyFactory;
            IdPropertyCache = new ConcurrentDictionary<Type, DynamicProperty>();
        }

        public abstract int? GetMemberRankingIndex(Type entityType, string membername);

        public virtual string GetValueFrom<T>(T entity)
        {
            return GetGetterFor(typeof (T)).GetValue(entity);
        }

        public void SetValueTo<T>(T entity, string value)
        {
            GetSetterFor(typeof(T)).SetValue(entity, value);
        }

        protected virtual DynamicStringGetter GetGetterFor(Type type)
        {
            return IdPropertyCache.GetOrAdd(
                type, 
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Getter;
        }

        protected virtual DynamicStringSetter GetSetterFor(Type type)
        {
            return IdPropertyCache.GetOrAdd(
                type,
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Setter;
        }

        protected virtual PropertyInfo GetPropertyFor(Type type)
        {
            return GetPropertiesFor(type).Select(p => new
            {
                PropertyInfo = p,
                Ranking = GetMemberRankingIndex(type, p.Name)
            })
            .Where(r => r.Ranking.HasValue)
            .OrderBy(r => r.Ranking.Value)
            .Select(r => r.PropertyInfo)
            .FirstOrDefault();
        }

        protected virtual IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
#if !NETFX_CORE
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
#else
            return type.GetRuntimeProperties();
#endif
        }
    }
}