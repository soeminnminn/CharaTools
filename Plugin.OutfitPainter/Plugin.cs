using CharaTools.AIChara;
using CharaTools.Plugin;
using OutfitPainter;

namespace Plugin.OutfitPainter
{
    public class Plugin : IPlugin
    {
        public string GUID => Controller.GUID;

        public bool CanDeserialize(string extKey) => extKey == GUID;

        public PluginData Serialize(string extKey, PluginData data, ExtendedPlugin file)
        {
            if (extKey == GUID)
            {
                //if (data.data.TryGetValue(Controller.ExtSaveKey, out object opData) && opData is OutfitPainterData)
                //    return Controller.SaveData(file, (OutfitPainterData)opData, false);
                //return data;
                return file.GetExtendedDataById(GUID);
            }
            return null;
        }

        public PluginData Deserialize(string extKey, ExtendedPlugin file)
        {
            try
            {
                var pluginData = new PluginData()
                {
                    version = 0
                };

                var data = Controller.LoadData(file);
                pluginData.data.Add(Controller.ExtSaveKey, data);
                return pluginData;
            }
            catch (Exception)
            {
                return file.GetExtendedDataById(GUID);
            }
        }

        public IPlugin.DataState GetDataState(string extKey, PluginData data)
        {
            if (extKey == GUID)
            {
                if (data.data.Count > 0 && data.data.ContainsKey(Controller.ExtSaveKey))
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