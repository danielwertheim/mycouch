using System;
using System.Collections.Generic;
#if NETFX_CORE
using System.Reflection;
#endif
using System.Linq;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    /// <summary>
    /// When deserializing and serializing with this contract resolver,
    /// Id and Rev members will be mapped according to conventions registrered
    /// in members of the <see cref="EntityReflector"/>.
    /// </summary>
    public class EntityContractResolver : SerializationContractResolver
    {
        protected readonly IEntityReflector EntityReflector;

#if NETFX_CORE
        private static readonly TypeInfo ResponseTypeInfo;
        private static readonly TypeInfo QueryResponseRowTypeInfo;

        static EntityContractResolver()
        {
            ResponseTypeInfo = typeof(Response).GetTypeInfo();
            QueryResponseRowTypeInfo = typeof(QueryResponseRow).GetTypeInfo();
        }
#endif

        public EntityContractResolver(IEntityReflector entityReflector)
        {
            Ensure.That(entityReflector, "entityReflector").IsNotNull();

            EntityReflector = entityReflector;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
#if !NETFX_CORE
            if (!type.IsClass || type == typeof(object) || typeof(Response).IsAssignableFrom(type) || typeof(QueryResponseRow).IsAssignableFrom(type))
                return base.CreateProperties(type, memberSerialization);
#else
            if(type == typeof(object))
                return base.CreateProperties(type, memberSerialization);

            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsClass || ResponseTypeInfo.IsAssignableFrom(typeInfo) || QueryResponseRowTypeInfo.IsAssignableFrom(typeInfo))
                return base.CreateProperties(type, memberSerialization);
#endif
            var props = base.CreateProperties(type, memberSerialization);
            if (!props.Any())
                return props;

            int? idRank = null, revRank = null;
            JsonProperty id = null, rev = null;

            foreach (var prop in props)
            {
                var tmpRank = EntityReflector.IdMember.GetMemberRankingIndex(type, prop.PropertyName);
                if (tmpRank != null)
                {
                    if (idRank == null || tmpRank < idRank)
                    {
                        idRank = tmpRank;
                        id = prop;
                    }

                    continue;
                }

                tmpRank = EntityReflector.RevMember.GetMemberRankingIndex(type, prop.PropertyName);
                if (tmpRank != null)
                {
                    if (revRank == null || tmpRank < revRank)
                    {
                        revRank = tmpRank;
                        rev = prop;
                    }
                }
            }

            if (id != null)
                id.PropertyName = "_id";

            if (rev != null)
                rev.PropertyName = "_rev";

            return props;
        }
    }
}