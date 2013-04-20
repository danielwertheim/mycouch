using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyCouch.Schemes
{
    public class EntityRevMember : EntityMember 
    {
        protected override IEnumerable<Func<PropertyInfo, bool>> GetDefaultPropertyLocators()
        {
            yield return p => p.Name.Equals("_rev", StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Equals(string.Concat(p.DeclaringType.Name, "rev"), StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Equals("entityrev", StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Equals("rev", StringComparison.OrdinalIgnoreCase);
        }
    }
}