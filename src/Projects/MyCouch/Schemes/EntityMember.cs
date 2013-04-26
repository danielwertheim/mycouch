using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyCouch.Reflections;

namespace MyCouch.Schemes
{
    public abstract class EntityMember : IEntityMember 
    {
        protected readonly ConcurrentDictionary<Type, DynamicProperty> IdPropertyCache;

        public BindingFlags PropertyBindingFlags { protected get; set; }

        protected EntityMember()
        {
            IdPropertyCache = new ConcurrentDictionary<Type, DynamicProperty>();
            PropertyBindingFlags = GetDefaultPropertyBindingFlags();
        }

        protected virtual BindingFlags GetDefaultPropertyBindingFlags()
        {
            return BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
        }

        public abstract int? GetMemberRankingIndex(Type entityType, string membername);

        public virtual string GetValueFrom<T>(T entity)
        {
            return GetGetterFor(typeof (T)).GetValue(entity) as string;
        }

        public void SetValueTo<T>(T entity, string value)
        {
            GetSetterFor(typeof(T)).SetValue(entity, value);
        }

        protected virtual DynamicGetter GetGetterFor(Type type)
        {
            return IdPropertyCache.GetOrAdd(
                type, 
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Getter;
        }

        protected virtual DynamicSetter GetSetterFor(Type type)
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
            return type.GetProperties(PropertyBindingFlags);
        }
    }
}