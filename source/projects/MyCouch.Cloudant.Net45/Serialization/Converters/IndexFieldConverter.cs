using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyCouch.Cloudant.Serialization.Converters
{
    public class IndexFieldConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IndexField);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadAsIndexField(reader);
        }

        protected virtual object ReadAsIndexField(JsonReader reader)
        {
            if (reader.TokenType != JsonToken.StartObject)
                return null;

            List<string> values = new List<string>();
            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                values.Add(reader.Value as string);

            return (values.Count == 2 && !string.IsNullOrWhiteSpace(values[0]) && !string.IsNullOrWhiteSpace(values[1])) ?
                new IndexField(values[0], values[1].AsSortDirection()) : null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = (IndexField)value;
            writer.WriteStartObject();
            writer.WritePropertyName(v.Name);
            writer.WriteValue(v.SortDirection.AsString());
            writer.WriteEndObject();
        }
    }
}
