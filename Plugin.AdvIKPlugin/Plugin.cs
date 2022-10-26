using CharaTools.AIChara;
using CharaTools.Plugin;
using MessagePack;

namespace Plugin.AdvIKPlugin
{
    public class Plugin : IPlugin
    {
        public string GUID => "orange.spork.advikplugin";

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
            try
            {
                if (extData != null)
                {
                    if (extData.data.TryGetValue("ResizeCentroid", out object resizeCentroidValue))
                    {
                        extData.data["ResizeCentroid"] = (IKResizeCentroid)resizeCentroidValue;
                    }

                    if (extData.data.TryGetValue("ResizeChainAdjustments", out object resizeChainAdjustmentsValue) && resizeChainAdjustmentsValue is byte[])
                    {
                        var ChainAdjustments = MessagePackSerializer.Deserialize<Dictionary<IKChain, IKResizeChainAdjustment>>((byte[])resizeChainAdjustmentsValue);
                        extData.data["ResizeChainAdjustments"] = ChainAdjustments; //.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());
                    }
                }
            }
            catch (Exception)
            { }

            return extData;
        }

        public IPlugin.DataState GetDataState(string extKey, PluginData data)
        {
            if (extKey == GUID)
            {
                if (data.data.Count > 0)
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