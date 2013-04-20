using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyCouch.Schemes
{
    public class EntityIdMember : EntityMember
    {
        protected override IEnumerable<Func<PropertyInfo, bool>> GetDefaultPropertyLocators()
        {
            yield return p => p.Name.Equals("_id", StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Equals(string.Concat(p.DeclaringType.Name, "id"), StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Equals("entityid", StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Equals("id", StringComparison.OrdinalIgnoreCase);
        }
    }
}