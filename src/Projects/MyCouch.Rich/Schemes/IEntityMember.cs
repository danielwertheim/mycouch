using System;

namespace MyCouch.Schemes
{
    public interface IEntityMember
    {
        int? GetMemberRankingIndex(Type entityType, string membername);
        string GetValueFrom<T>(T entity);
        void SetValueTo<T>(T entity, string value);
    }
}