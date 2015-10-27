﻿using System;
using System.IO;
#if PCL || vNext
using System.Reflection;
#endif
using MyCouch.EnsureThat;
using MyCouch.EntitySchemes;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class DocumentJsonReader : JsonTextReader
    {
        private const string CouchIdMemberName = "_id";
        private const string CouchRevMemberName = "_rev";

        protected Type DocType { get; private set; }
        protected readonly IEntityReflector EntityReflector;
        protected readonly TextReader Reader;
        protected int Level { get; private set; }
        protected bool HasTranslatedId { get; private set; }
        protected bool HasTranslatedRev { get; private set; }
#if PCL || vNext
        protected readonly TypeInfo DocTypeInfo;
#endif

        public DocumentJsonReader(TextReader reader, Type docType, IEntityReflector entityReflector)
            : base(reader)
        {
            Ensure.That(docType, "docType").IsNotNull();
            Ensure.That(entityReflector, "entityReflector").IsNotNull();

            DocType = docType;
            EntityReflector = entityReflector;
            Reader = reader;
            CloseInput = false;
#if PCL || vNext
            DocTypeInfo = docType.GetTypeInfo();
#endif
        }

        public override bool Read()
        {
            var r = base.Read();

            if (TokenType == JsonToken.StartObject)
            {
                Level = Level += 1;
                return r;
            }

            if (!ShouldTranslate())
                return r;

            TranslatePropertyName(Value.ToString());

            return r;
        }

        private void TranslatePropertyName(string name)
        {
            if(TokenType != JsonToken.PropertyName)
                return;

            if (name == CouchIdMemberName)
            {
                var idMemberName = EntityReflector.IdMember.GetPropertyNameFor(DocType);
                if (idMemberName != null)
                {
                    SetToken(JsonToken.PropertyName, idMemberName);
                    HasTranslatedId = true;
                    return;
                }
            }

            if (name == CouchRevMemberName)
            {
                var revMemberName = EntityReflector.RevMember.GetPropertyNameFor(DocType);
                if (revMemberName != null)
                {
                    SetToken(JsonToken.PropertyName, revMemberName);
                    HasTranslatedRev = true;
                    return;
                }
            }
        }

        private bool ShouldTranslate()
        {
            if (Level > 1)
                return false;

            if (TokenType != JsonToken.PropertyName)
                return false;

            if (HasTranslatedId && HasTranslatedRev)
                return false;

            if (DocType == typeof(object))
                return false;

#if PCL || vNext
            if (!DocTypeInfo.IsClass)
                return false;
#else
            if (!DocType.IsClass)
                return false;
#endif

            return true;
        }

        public virtual void RunWithNewTempRoot(Type docType, Action a)
        {
            var snapshot = new ResetInfo(DocType, HasTranslatedId, HasTranslatedRev);

            try
            {
                DocType = docType;
                Level = 0;
                HasTranslatedId = false;
                HasTranslatedRev = false;
                
                a();
            }
            finally 
            {
                DocType = snapshot.DocType;
                HasTranslatedId = snapshot.HasTranslatedId;
                HasTranslatedRev = snapshot.HasTranslatedRev;
            }
        }

        private class ResetInfo
        {
            public Type DocType { get; private set; }
            public bool HasTranslatedId { get; private set; }
            public bool HasTranslatedRev { get; private set; }

            internal ResetInfo(Type docType, bool hasTranslatedId, bool hasTranslatedRev)
            {
                DocType = docType;
                HasTranslatedId = hasTranslatedId;
                HasTranslatedRev = hasTranslatedRev;
            }
        }
    }
}
