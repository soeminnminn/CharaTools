using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CharaTools.Json
{
    public class ByteArrayConverter : JsonConverter<byte[]>
    {
        private BinaryConverter binaryConverter = new BinaryConverter();

        public override byte[] ReadJson(JsonReader reader, Type objectType, byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return binaryConverter.ReadJson(reader, objectType, existingValue, serializer) as byte[];
        }

        public override void WriteJson(JsonWriter writer, byte[] value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteStartArray();
            foreach (var v in value)
                writer.WriteValue(v);
            writer.WriteEndArray();
        }
    }
}
