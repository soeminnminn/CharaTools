using System;
using System.Linq;
using Newtonsoft.Json;

namespace CharaTools.Json
{
    public class ParameterIntConverter : JsonConverter<int>
    {
        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (!string.IsNullOrEmpty(reader.Path))
            {
                var value = reader.ReadAsString();
                if (!string.IsNullOrEmpty(value))
                {
                    if (reader.Path.EndsWith("personality"))
                    {
                        var idx = Constants.ssPersonality.ToList().IndexOf(value);
                        return Math.Max(0, idx);
                    }
                    else if (reader.Path.EndsWith("trait"))
                    {
                        var idx = Constants.ssTrait.ToList().IndexOf(value);
                        return Math.Max(0, idx);
                    }
                    else if (reader.Path.EndsWith("mind"))
                    {
                        var idx = Constants.ssMentality.ToList().IndexOf(value);
                        return Math.Max(0, idx);
                    }
                    else if (reader.Path.EndsWith("hAttribute"))
                    {
                        var idx = Constants.ssSexTrait.ToList().IndexOf(value);
                        return Math.Max(0, idx);
                    }
                }
            }
            return 0;
        }

        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
        {
            if (!string.IsNullOrEmpty(writer.Path))
            {
                if (writer.Path.EndsWith("personality") && Constants.ssPersonality.Length > value)
                {
                    writer.WriteValue(Constants.ssPersonality[value]);
                    return;
                }
                else if (writer.Path.EndsWith("trait") && Constants.ssTrait.Length > value)
                {
                    writer.WriteValue(Constants.ssTrait[value]);
                    return;
                }
                else if (writer.Path.EndsWith("mind") && Constants.ssMentality.Length > value)
                {
                    writer.WriteValue(Constants.ssMentality[value]);
                    return;
                }
                else if (writer.Path.EndsWith("hAttribute") && Constants.ssSexTrait.Length > value)
                {
                    writer.WriteValue(Constants.ssSexTrait[value]);
                    return;
                }
            }
            writer.WriteValue(value);
        }
    }
}
