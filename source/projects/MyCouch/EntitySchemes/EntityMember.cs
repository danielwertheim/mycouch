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
        protected IDynamicPropertyFactory DynamicPropertyFactory { get; private set; }
        protected ConcurrentDictionary<Type, DynamicProperty> DynamicPropertyCache { get; private set; }
        protected EntityMember(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            Ensure.That(dynamicPropertyFactory, "dynamicPropertyFactory").IsNotNull();

            DynamicPropertyFactory = dynamicPropertyFactory;

            DynamicPropertyCache = new ConcurrentDictionary<Type, DynamicProperty>();
        }

        public abstract int? GetMemberRankingIndex(Type entityType, string membername);

        public virtual string GetValueFrom<T>(T entity) where T : class
        {
            return entity == null
                ? default(string)
                : GetGetterFor(typeof(T)).GetValue(entity);
        }

        public void SetValueTo<T>(T entity, string value) where T : class
        {
            if(entity == null)
                return;

            GetSetterFor(typeof(T)).SetValue(entity, value);
        }

        protected virtual IStringGetter GetGetterFor(Type type)
        {
            return DynamicPropertyCache.GetOrAdd(
                type, 
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Getter;
        }

        protected virtual IStringSetter GetSetterFor(Type type)
        {
            return DynamicPropertyCache.GetOrAdd(
                type,
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Setter;
        }

        public virtual string GetPropertyNameFor(Type type)
        {
            return DynamicPropertyCache.GetOrAdd(
                type,
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Name;
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
            return type.GetRuntimeProperties();
        }
    }
}