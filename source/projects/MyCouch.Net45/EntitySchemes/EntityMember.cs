using System;
#if !PCL
using System.Collections.Concurrent;
#endif
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
#if !PCL
        protected ConcurrentDictionary<Type, DynamicProperty> IdPropertyCache { get; private set; }
#else
        protected Dictionary<Type, DynamicProperty> IdPropertyCache { get; private set; }
#endif
        protected EntityMember(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            Ensure.That(dynamicPropertyFactory, "dynamicPropertyFactory").IsNotNull();

            DynamicPropertyFactory = dynamicPropertyFactory;
#if !PCL
            IdPropertyCache = new ConcurrentDictionary<Type, DynamicProperty>();
#else
            IdPropertyCache = new Dictionary<Type, DynamicProperty>();
#endif
        }

        public abstract int? GetMemberRankingIndex(Type entityType, string membername);

        public virtual string GetValueFrom<T>(T entity)
        {
            return GetGetterFor(typeof(T)).GetValue(entity);
        }

        public void SetValueTo<T>(T entity, string value)
        {
            GetSetterFor(typeof(T)).SetValue(entity, value);
        }

        protected virtual IStringGetter GetGetterFor(Type type)
        {
#if !PCL
            return IdPropertyCache.GetOrAdd(
                type, 
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Getter;
#else
            if (IdPropertyCache.ContainsKey(type))
                return IdPropertyCache[type].Getter;

            lock (IdPropertyCache)
            {
                if (IdPropertyCache.ContainsKey(type))
                    return IdPropertyCache[type].Getter;

                var r = DynamicPropertyFactory.PropertyFor(GetPropertyFor(type));
                IdPropertyCache.Add(type, r);

                return r.Getter;
            }
#endif
        }

        protected virtual IStringSetter GetSetterFor(Type type)
        {
#if !PCL
            return IdPropertyCache.GetOrAdd(
                type,
                t => DynamicPropertyFactory.PropertyFor(GetPropertyFor(type))).Setter;
#else
            if (IdPropertyCache.ContainsKey(type))
                return IdPropertyCache[type].Setter;

            lock (IdPropertyCache)
            {
                if (IdPropertyCache.ContainsKey(type))
                    return IdPropertyCache[type].Setter;

                var r = DynamicPropertyFactory.PropertyFor(GetPropertyFor(type));
                IdPropertyCache.Add(type, r);

                return r.Setter;
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
#if !PCL
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
#else
            return type.GetRuntimeProperties();
#endif
        }
    }
}