using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CharaTools.AIChara;
using MessagePack;

namespace KKABMX.Core
{
    public class ABMData
    {
        public const string ExtDataGUID = "KKABMPlugin.ABMData";
        public const string ExtDataBoneDataKey = "boneData";
        public const int ExtDataVersion = 2;

        public static Dictionary<string, BoneModifierData> Deserialize(ExtendedPlugin file)
        {
            var data = file.GetExtendedDataById(ExtDataGUID);
            if (data != null)
            {
                // var boneData = LZ4MessagePackSerializer.Deserialize<Dictionary<string, BoneModifierData>>((byte[])data.data[ExtDataBoneDataKey]);

                if (data.data.TryGetValue(ExtDataBoneDataKey, out object value) && value is byte[])
                {
                    return Deserialize((byte[])value);
                }
            }
            return null;
        }

        public static Dictionary<string, BoneModifierData> Deserialize(byte[] data)
        {
            Dictionary<string, BoneModifierData> dict = new Dictionary<string, BoneModifierData>();
            var options = MessagePackSerializerOptions.Standard
                            .WithCompression(MessagePackCompression.Lz4BlockArray)
                            .WithResolver(MessagePack.Resolvers.StandardResolverAllowPrivate.Instance);

            if (data != null)
            {
                try
                {
                    var deObj = MessagePackSerializer.Deserialize<KeyValuePair[]>(data, options);
                    if (deObj != null)
                    {
                        foreach(var i in deObj)
                        {
                            if (i.Value != null && i.Value.Length > 0)
                                dict.Add(i.Key, i.Value[0]);
                            else
                                dict.Add(i.Key, null);
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }

            return dict;
        }

        public static void Serialize(ExtendedPlugin file, Dictionary<string, BoneModifierData> data)
        {
            var toSave = data.Select(x => 
            {
                var item = new KeyValuePair() 
                {
                    Key = x.Key,
                    Value = new BoneModifierData[1] { x.Value }
                };
                return item;
            }).ToArray();

            var options = MessagePackSerializerOptions.Standard
                            .WithCompression(MessagePackCompression.Lz4BlockArray)
                            .WithResolver(MessagePack.Resolvers.StandardResolverAllowPrivate.Instance);

            var pluginData = new PluginData { version = 2 };
            // pluginData.data.Add(ExtDataBoneDataKey, LZ4MessagePackSerializer.Serialize(toSave));
            // SetCoordinateExtendedData(coordinate, pluginData);

            pluginData.data.Add(ExtDataBoneDataKey, MessagePackSerializer.Serialize(toSave, options));
            file.SetExtendedDataById(ExtDataGUID, pluginData);
        }

        #region Nested Types
        [MessagePackObject]
        public class KeyValuePair
        {
            [Key(0)]
            public string Key;

            [Key(1)]
            public BoneModifierData[] Value;
        }
        #endregion
    }
}
