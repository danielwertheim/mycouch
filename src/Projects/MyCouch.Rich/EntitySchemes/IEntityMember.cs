using System;

namespace MyCouch.Rich.EntitySchemes
{
    public interface IEntityMember
    {
        int? GetMemberRankingIndex(Type entityType, string membername);
        string GetValueFrom<T>(T entity);
        void SetValueTo<T>(T entity, string value);
    }
}