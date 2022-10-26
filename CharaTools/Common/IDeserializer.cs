using System;
using CharaTools.AIChara;

namespace CharaTools
{
    public interface IDeserializer
    {
        bool CanDeserialize(string extKey);

        PluginData Serialize(string extKey, PluginData data, ExtendedPlugin file);

        PluginData Deserialize(string extKey, ExtendedPlugin file);
    }
}
