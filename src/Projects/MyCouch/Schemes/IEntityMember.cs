using System;
using System.Reflection;

namespace MyCouch.Schemes
{
    public interface IEntityMember
    {
        BindingFlags PropertyBindingFlags { set; }
        int? GetMemberRankingIndex(Type entityType, string membername);
        string GetValueFrom<T>(T entity);
        void SetValueTo<T>(T entity, string value);
    }
}