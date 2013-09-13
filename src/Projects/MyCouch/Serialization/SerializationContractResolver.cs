using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyCouch.Extensions;
using MyCouch.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class SerializationContractResolver : DefaultContractResolver
    {
        public SerializationContractResolver() : base(true) { }

        protected override string ResolvePropertyName(string propertyName)
        {
            return base.ResolvePropertyName(propertyName.ToCamelCase());
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
#if !NETFX_CORE
            if (!typeof(QueryResponseRow).IsAssignableFrom(type))
                return base.CreateProperties(type, memberSerialization);
#else
            if (!QueryResponseRow.TypeInfo.IsAssignableFrom(type.GetTypeInfo()))
                return base.CreateProperties(type, memberSerialization);
#endif
            return base.CreateProperties(type, memberSerialization).Select(p =>
            {
                if (p.PropertyName == "includedDoc")
                    p.PropertyName = "doc";

                return p;
            }).ToList();
        }
    }
}