using System;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Converters
{
    public class UnixEpochDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException(ExceptionStrings.JsonConverterDoesNotSupportSerialization);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.Value == null)
                return null;

            var microseconds = reader.Value is string
                ? long.Parse((string)reader.Value)
                : (long)reader.Value;

            if (microseconds == 0)
                return null;

            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddMilliseconds(microseconds / 1000);
        }
    }
}