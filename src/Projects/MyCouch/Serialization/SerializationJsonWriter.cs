using System;
using System.IO;
using MyCouch.Serialization.Conventions;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class SerializationJsonWriter : JsonTextWriter
    {
        protected bool HasWrittenDocHeader = false;
        protected bool ShouldSkipOneStartObjectWrite = false;

        public DocSerializationConventions Conventions { get; set; }

        public SerializationJsonWriter(TextWriter textWriter)
            : base(textWriter)
        {
            Conventions = new DocSerializationConventions();
        }

        public virtual void WriteDocHeaderFor<T>(T doc)
        {
            if(HasWrittenDocHeader)
                return;

            WriteStartObject();
            WriteDocType(typeof (T));

            HasWrittenDocHeader = true;
            ShouldSkipOneStartObjectWrite = true;
        }

        public override void WriteStartObject()
        {
            if (HasWrittenDocHeader && ShouldSkipOneStartObjectWrite)
            {
                ShouldSkipOneStartObjectWrite = false;
                return;
            }

            base.WriteStartObject();
        }

        protected virtual void WriteDocType(Type docType)
        {
            WritePropertyName(Conventions.DocType.PropertyName);
            WriteValue(Conventions.DocType.Convention(docType));
        }
    }
}