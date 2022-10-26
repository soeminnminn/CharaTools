using System;
using CharaTools.AIChara;

namespace CharaTools.Plugin
{
    public interface IPlugin : IDeserializer
    {
        string GUID { get; }

        void OnPluginLoaded();

        void OnChaFileLoaded(ChaFile file);

        void OnChaFileBeforeSave(ChaFile file);

        void OnCoordinateLoaded(ChaFileCoordinate file);

        DataState GetDataState(string extKey, PluginData data);

        public enum DataState
        {
            NotSupported,
            IsEmpty,
            DataPresent
        }
    }
}
