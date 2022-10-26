using CharaTools.AIChara;
using CharaTools.Plugin;
using AdditionalAccessoryControls;

namespace Plugin.AdditionalAccessoryControls
{
    public class Plugin : Controller, IPlugin
    {
        public new string GUID => Controller.GUID;

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
            LoadFromCoordinate(file);

            var pluginData = new PluginData();
            pluginData.data.Add(accessoryDataKey, SlotData);
            pluginData.data.Add(overrideDataKey, CoordinateOverrideData);

            return pluginData;
        }

        public IPlugin.DataState GetDataState(string extKey, PluginData data)
        {
            if (extKey == GUID)
            {
                if (data.data.Count == 0) 
                    return IPlugin.DataState.IsEmpty;

                data.data.TryGetValue(accessoryDataKey, out object d1);
                data.data.TryGetValue(overrideDataKey, out object d2);

                if (d1 == null && d2 == null)
                    return IPlugin.DataState.IsEmpty;

                return IPlugin.DataState.DataPresent;
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