using Newtonsoft.Json;
using System;

namespace MyCouch.Cloudant.Serialization.Converters
{
    public class IndexFieldConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IndexField);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
