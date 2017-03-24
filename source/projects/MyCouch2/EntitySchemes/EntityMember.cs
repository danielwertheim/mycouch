using System;
#if !PCL
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyCouch.EnsureThat;
using MyCouch.EntitySchemes.Reflections;

namespace MyCouch.EntitySchemes
{
    public abstract class EntityMember : IEntityMember
    {
        protected IDynamicPropertyFactory DynamicPropertyFactory { get; private set; }
#if !PCL
        protected ConcurrentDictionary<Type, DynamicProperty> DynamicPropertyCache { get; private set; }
#else
        protected Dictionary<Type, DynamicProperty> DynamicPropertyCache { get; private set; }
#endif
        protected EntityMember(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            Ensure.That(dynamicPropertyFactory, "dynamicPropertyFactory").IsNotNull();

            DynamicPropertyFactory = dynamicPropertyFactory;
#if !PCL
            DynamicPropertyCache = new ConcurrentDictionary<Type, DynamicProperty>();
#else
            DynamicPropertyCache = new Dictionary<Type, DynamicProperty>();
#endif
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
#if !PCL
            return DynamicPropertyCache.GetOrAdd(
                type, 
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Getter;
#else
            if (DynamicPropertyCache.ContainsKey(type))
                return DynamicPropertyCache[type].Getter;

            lock (DynamicPropertyCache)
            {
                if (DynamicPropertyCache.ContainsKey(type))
                    return DynamicPropertyCache[type].Getter;

                var r = DynamicPropertyFactory.PropertyFor(GetPropertyFor(type));
                DynamicPropertyCache.Add(type, r);

                return r.Getter;
            }
#endif
        }

        protected virtual IStringSetter GetSetterFor(Type type)
        {
#if !PCL
            return DynamicPropertyCache.GetOrAdd(
                type,
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Setter;
#else
            if (DynamicPropertyCache.ContainsKey(type))
                return DynamicPropertyCache[type].Setter;

            lock (DynamicPropertyCache)
            {
                if (DynamicPropertyCache.ContainsKey(type))
                    return DynamicPropertyCache[type].Setter;

                var r = DynamicPropertyFactory.PropertyFor(GetPropertyFor(type));
                DynamicPropertyCache.Add(type, r);

                return r.Setter;
            }
#endif
        }

        public virtual string GetPropertyNameFor(Type type)
        {
#if !PCL
            return DynamicPropertyCache.GetOrAdd(
                type,
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Name;
#else
            if (DynamicPropertyCache.ContainsKey(type))
                return DynamicPropertyCache[type].Name;

            lock (DynamicPropertyCache)
            {
                if (DynamicPropertyCache.ContainsKey(type))
                    return DynamicPropertyCache[type].Name;

                var r = DynamicPropertyFactory.PropertyFor(GetPropertyFor(type));
                DynamicPropertyCache.Add(type, r);

                return r.Name;
            }
#endif
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
#if net45
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
#else
            return type.GetRuntimeProperties();
#endif
        }
    }
}