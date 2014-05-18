using System;
using System.Reflection;

namespace MyCouch.EntitySchemes
{
    public interface IEntityMember
    {
        PropertyInfo GetPropertyFor(Type type);
        int? GetMemberRankingIndex(Type entityType, string membername);
        string GetValueFrom<T>(T entity) where T : class;
        void SetValueTo<T>(T entity, string value) where T : class;
    }
}