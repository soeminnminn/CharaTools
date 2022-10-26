using CharaTools.AIChara;
using CharaTools.Plugin;

namespace Plugin.ABMX
{
    public class Plugin : IPlugin
    {
        public string GUID => KKABMX.Core.ABMData.ExtDataGUID;

        public bool CanDeserialize(string extKey) => extKey == KKABMX.Core.ABMData.ExtDataGUID;

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
            var extData = file.GetExtendedDataById(KKABMX.Core.ABMData.ExtDataGUID);
            try
            {
                var deObj = KKABMX.Core.ABMData.Deserialize(file);
                if (deObj != null)
                {
                    PluginData pluginData = new PluginData()
                    {
                        version = KKABMX.Core.ABMData.ExtDataVersion
                    };
                    pluginData.data.Add(KKABMX.Core.ABMData.ExtDataBoneDataKey, deObj);
                    return pluginData;
                }
            }
            catch (Exception)
            {
            }
            return extData;
        }

        public IPlugin.DataState GetDataState(string extKey, PluginData data)
        {
            if (extKey == KKABMX.Core.ABMData.ExtDataGUID)
            {
                if (data.data.ContainsKey(KKABMX.Core.ABMData.ExtDataBoneDataKey))
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