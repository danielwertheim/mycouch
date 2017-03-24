using System;
#if !PCL
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif
using System.Reflection;

namespace MyCouch.Serialization.Meta
{
    public class DocumentSerializationMetaProvider : IDocumentSerializationMetaProvider
    {
        protected const string AnonymousTypePrefix = "<>";
#if !PCL
        protected ConcurrentDictionary<Type, DocumentSerializationMeta> Cache { get; private set; }
#else
        protected Dictionary<Type, DocumentSerializationMeta> Cache { get; private set; }
#endif

        public DocumentSerializationMetaProvider()
        {
#if !PCL
            Cache = new ConcurrentDictionary<Type, DocumentSerializationMeta>();
#else
            Cache = new Dictionary<Type, DocumentSerializationMeta>();
#endif
        }

        public virtual DocumentSerializationMeta Get(Type docType)
        {
#if !PCL
            return Cache.GetOrAdd(docType, CreateFor);
#else
            if (Cache.ContainsKey(docType))
                return Cache[docType];

            lock (Cache)
            {
                if (Cache.ContainsKey(docType))
                    return Cache[docType];

                var r = CreateFor(docType);
                Cache.Add(docType, r);

                return r;
            }
#endif
        }

        protected virtual DocumentSerializationMeta CreateFor(Type docType)
        {
            var isAnonymous = CheckIfDocTypeIsAnonymous(docType);
            var metaAttr = ExtractMetaDataAttribute(docType);
            var type = metaAttr == null ? docType.Name : metaAttr.DocType ?? docType.Name;

            var meta = new DocumentSerializationMeta(docType, type, isAnonymous);
            if (metaAttr != null)
            {
                meta.DocNamespace = metaAttr.DocNamespace;
                meta.DocVersion = metaAttr.DocVersion;
            }

            return meta;
        }

        protected virtual DocumentAttribute ExtractMetaDataAttribute(Type docType)
        {
#if NETSTANDARD1_1 || vNext || PCL
            return docType.GetTypeInfo().GetCustomAttribute<DocumentAttribute>();
#else
            return docType.GetCustomAttribute<DocumentAttribute>(true);
#endif
        }

        protected virtual bool CheckIfDocTypeIsAnonymous(Type docType)
        {
#if NETSTANDARD1_1 || vNext || PCL
            var info = docType.GetTypeInfo();

            return info.IsClass &&
                info.IsNotPublic &&
                info.IsSealed &&
                info.IsGenericType &&
                info.BaseType == typeof(object) &&
                info.Namespace == null &&
                info.Name.StartsWith(AnonymousTypePrefix);
#else
            return docType.IsClass &&
                docType.IsNotPublic &&
                docType.IsSealed &&
                docType.IsGenericType &&
                docType.BaseType == typeof(object) &&
                docType.Namespace == null &&
                docType.Name.StartsWith(AnonymousTypePrefix);
#endif
        }
    }
}