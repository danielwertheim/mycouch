using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Converters
{
    /// <summary>
    /// Used on specific properties that needs to support deserialization
    /// to string or string-array or entity.
    /// </summary>
    public class MultiTypeDeserializationJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException(ExceptionStrings.JsonConverterDoesNotSupportSerialization);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof (string))
                return serializer.Deserialize(reader, objectType);

            return ReadJsonAsString(reader);
        }

        protected virtual object ReadJsonAsString(JsonReader reader)
        {
            var sb = new StringBuilder();

            using (var wo = new StringWriter(sb))
            {
                using (var w = new JsonTextWriter(wo))
                {
                    w.WriteToken(reader, true);
                }
            }

            return sb.ToString();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}