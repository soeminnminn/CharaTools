using System;
using System.Linq;
using Newtonsoft.Json;

namespace CharaTools.Json
{
    public class ParameterByteConverter : JsonConverter<byte>
    {
        private ParameterIntConverter converter = new ParameterIntConverter();

        public override byte ReadJson(JsonReader reader, Type objectType, byte existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (byte)converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, byte value, JsonSerializer serializer)
        {
            converter.WriteJson(writer, value, serializer);
        }
    }
}
