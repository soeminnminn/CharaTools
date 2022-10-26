using System;
using System.Collections.Generic;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    public abstract class ExtendedPlugin
    {
        [JsonIgnore]
        [IgnoreMember]
        public int kkExVersion = 3;

        [JsonIgnore]
        [IgnoreMember]
        public Dictionary<string, PluginData> ExtendedData { get; set; } = new Dictionary<string, PluginData>();

        public void SetExtendedBytes(byte[] data, string version)
        {
            if (int.TryParse(version, out int ver))
                kkExVersion = ver;

            ExtendedData = MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(data);
        }

        public byte[] GetExtendedBytes()
        {
            var extendedData = ExtendedData;
            if (extendedData == null) return null;

            List<string> keysToRemove = new List<string>();

            foreach (var entry in extendedData)
                if (entry.Value == null)
                    keysToRemove.Add(entry.Key);

            foreach (var key in keysToRemove)
                extendedData.Remove(key);

            return MessagePackSerializer.Serialize(extendedData);
        }

        /// <summary>
        /// Get PluginData for a ChaFile for the specified extended save data ID
        /// </summary>
        /// <param name="id">ID of the data saved to the card</param>
        /// <returns>PluginData</returns>
        public PluginData GetExtendedDataById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return ExtendedData != null && ExtendedData.TryGetValue(id, out var extendedSection) ? extendedSection : null;
        }

        /// <summary>
        /// Set PluginData for a ChaFile for the specified extended save data ID
        /// </summary>
        /// <param name="id">ID of the data to be saved to the card</param>
        /// <param name="extendedFormatData">PluginData to save to the card</param>
        public void SetExtendedDataById(string id, PluginData extendedFormatData)
        {
            if (ExtendedData == null)
                ExtendedData = new Dictionary<string, PluginData>();

            if (ExtendedData.ContainsKey(id))
                ExtendedData[id] = extendedFormatData;
            else
                ExtendedData.Add(id, extendedFormatData);
        }
    }
}
