using System;
using System.Collections.Generic;
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
            if (objectType == typeof(string))
                return ReadJsonAsString(reader);

            if (objectType == typeof(string[]))
                return ReadJsonAsStringArray(reader);

            return serializer.Deserialize(reader, objectType);
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

        protected virtual object ReadJsonAsStringArray(JsonReader reader)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                reader.Skip();
                return null;
            }

            var rowValues = new List<string>();
            var valueStartDepth = reader.Depth;
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    while (reader.Read() && !(reader.TokenType == JsonToken.EndArray && reader.Depth == valueStartDepth))
                    {
                        jw.WriteToken(reader, true);
                        rowValues.Add(sb.ToString());
                        sb.Clear();
                    }
                }
            }

            return rowValues.ToArray();
        }


        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}