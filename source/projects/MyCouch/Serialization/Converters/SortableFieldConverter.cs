using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyCouch.Serialization.Converters
{
    public class SortableFieldConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SortableField);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadAsSortableField(reader);
        }

        protected virtual object ReadAsSortableField(JsonReader reader)
        {
            if (reader.TokenType != JsonToken.StartObject)
                return null;

            var values = new List<string>();
            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                values.Add(reader.Value as string);

            return (values.Count == 2 && !string.IsNullOrWhiteSpace(values[0]) && !string.IsNullOrWhiteSpace(values[1]))
                ? new SortableField(values[0], values[1].AsSortDirection())
                : null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = (SortableField)value;
            writer.WriteStartObject();
            writer.WritePropertyName(v.Name);
            writer.WriteValue(v.SortDirection.AsString());
            writer.WriteEndObject();
        }
    }
}
