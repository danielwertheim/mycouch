using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Converters
{
    public class KeyJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException(ExceptionStrings.JsonConverterDoesNotSupportSerialization);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
                return ReadAsObjectArray(reader);

            return reader.Value;
        }

        protected virtual object[] ReadAsObjectArray(JsonReader reader)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                reader.Skip();
                return null;
            }

            var rowValues = new List<object>();
            var valueStartDepth = reader.Depth;

            while (reader.Read() && !(reader.TokenType == JsonToken.EndArray && reader.Depth == valueStartDepth))
            {
                rowValues.Add(reader.Value);
            }

            return rowValues.ToArray();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }
    }
}