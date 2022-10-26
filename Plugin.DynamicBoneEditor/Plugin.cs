using CharaTools.AIChara;
using CharaTools.Plugin;
using MessagePack;

namespace Plugin.DynamicBoneEditor
{
    public class Plugin : IPlugin
    {
        public string GUID => "com.deathweasel.bepinex.dynamicboneeditor";

        public bool CanDeserialize(string extKey) => extKey == GUID;

        public PluginData Serialize(string extKey, PluginData data, ExtendedPlugin file)
        {
            if (extKey == GUID)
            {
                return file.GetExtendedDataById(GUID);
            }
            return data;
        }

        public PluginData Deserialize(string extKey, ExtendedPlugin file)
        {
            var extData = file.GetExtendedDataById(GUID);
            if (extData.data.TryGetValue("AccessoryDynamicBoneData", out object value) && value is byte[])
            {
                var pluginData = new PluginData()
                {
                    version = extData.version
                };

                try
                {
                    List<DynamicBoneData> accessoryDynamicBoneData = MessagePackSerializer.Deserialize<List<DynamicBoneData>>((byte[])value);
                    // var deObj = MessagePackSerializer.Typeless.Deserialize((byte[])value);                
                    pluginData.data.Add("AccessoryDynamicBoneData", accessoryDynamicBoneData);
                    return pluginData;
                }
                catch (Exception)
                {
                    return extData;
                }
            }
            else
                return extData;
        }

        public IPlugin.DataState GetDataState(string extKey, PluginData data)
        {
            if (extKey == GUID)
            {
                if (data.data.Count > 0 && data.data.ContainsKey("AccessoryDynamicBoneData"))
                    return IPlugin.DataState.DataPresent;

                return IPlugin.DataState.IsEmpty;
            }
            return IPlugin.DataState.NotSupported;
        }

        public void OnChaFileBeforeSave(ChaFile file)
        {
        }

        public void OnChaFileLoaded(ChaFile file)
        {
        }

        public void OnCoordinateLoaded(ChaFileCoordinate file)
        {
        }

        public void OnPluginLoaded()
        {
        }
    }
}