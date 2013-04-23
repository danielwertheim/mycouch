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
        public IList<Func<PropertyInfo, bool>> PropertyLocatorPredicates { get; protected set; }

        protected EntityMember()
        {
            IdPropertyCache = new ConcurrentDictionary<Type, DynamicProperty>();
            PropertyBindingFlags = GetDefaultPropertyBindingFlags();
            PropertyLocatorPredicates = GetDefaultPropertyLocators().ToList();
        }

        protected virtual BindingFlags GetDefaultPropertyBindingFlags()
        {
            return BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
        }

        protected abstract IEnumerable<Func<PropertyInfo, bool>> GetDefaultPropertyLocators();

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
            return GetPropertiesFor(type).FirstOrDefault(property => PropertyLocatorPredicates.Any(predicate => predicate(property)));
        }

        protected virtual IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return type.GetProperties(PropertyBindingFlags);
        }
    }
}