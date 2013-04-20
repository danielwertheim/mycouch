using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyCouch.Schemes
{
    public interface IEntityMember
    {
        BindingFlags PropertyBindingFlags { set; }
        IList<Func<PropertyInfo, bool>> PropertyLocatorPredicates { get; }
        string GetValueFrom<T>(T entity);
        void SetValueTo<T>(T entity, string value);
    }
}