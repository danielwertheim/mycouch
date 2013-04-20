using System.Linq;
using System.Reflection;
using MyCouch.Extensions;
using MyCouch.Schemes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class SerializationContractResolver : DefaultContractResolver
    {
        protected readonly IEntityAccessor EntityAccessor;

        public SerializationContractResolver(IEntityAccessor entityAccessor)
            : base(true)
        {
            EntityAccessor = entityAccessor;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!(member is PropertyInfo))
                return prop;

            if (EntityAccessor.IdMember.PropertyLocatorPredicates.Any(p => p((PropertyInfo) member)))
                prop.PropertyName = "_id";
            else if (EntityAccessor.RevMember.PropertyLocatorPredicates.Any(p => p((PropertyInfo)member)))
                prop.PropertyName = "_rev";

            return prop;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return base.ResolvePropertyName(propertyName.ToCamelCase());
        }
    }
}