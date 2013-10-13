using System;
using System.Collections.Generic;
using System.Linq;
using MyCouch.Extensions;
using MyCouch.Serialization.Converters;
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
            return base.CreateProperties(type, memberSerialization).Select(p =>
            {
                if (p.PropertyName == "includedDoc")
                {
                    p.MemberConverter = new MultiTypeDeserializationJsonConverter();
                    p.PropertyName = "doc";
                    return p;
                }

                if (p.PropertyName == "lastSeq")
                {
                    p.PropertyName = "last_seq";
                    return p;
                }

                return p;
            }).ToList();
        }
    }
}