using CharaTools.AIChara;
using CharaTools.Plugin;
using MessagePack;

namespace Plugin.OverlayMods
{
    public class Plugin : IPlugin
    {
        public const string Version = "6.0.7";
        public const string GUID_KSOX = "KSOX";
        public const string GUID_KCOX = "KCOX";

        private const string DataMarker = "_TextureID_";
        private const string LookupDataKey = "Lookup";
        private const string OverlayDataKey = "Overlays";

        public enum TexType
        {
            Unknown = 0,
            BodyOver = 1,
            FaceOver = 2,
            BodyUnder = 3,
            FaceUnder = 4,
            /// <summary>
            /// Same as using both EyeUnderL and EyeUnderR
            /// </summary>
            EyeUnder = 5,
            /// <summary>
            /// Same as using both EyeOverL and EyeOverR
            /// </summary>
            EyeOver = 6,
            EyeUnderL = 7,
            EyeOverL = 8,
            EyeUnderR = 9,
            EyeOverR = 10
        }

        public enum CoordinateType
        {
            Unknown
        }

        public string GUID => $"{GUID_KSOX}.{GUID_KCOX}";

        public bool CanDeserialize(string extKey)
        {
            return extKey == GUID_KSOX || extKey == GUID_KCOX;
        }

        public PluginData Serialize(string extKey, PluginData data, ExtendedPlugin file)
        {
            if (extKey == GUID_KSOX)
            {
                return file.GetExtendedDataById(GUID_KSOX);
            }
            else if (extKey == GUID_KCOX)
            { 
                return file.GetExtendedDataById(GUID_KCOX);
            }
            return data;
        }

        public PluginData Deserialize(string extKey, ExtendedPlugin file)
        {
            var pd = new PluginData { version = 2 };

            if (extKey == GUID_KSOX)
            {
                var extData = file.GetExtendedDataById(GUID_KSOX);
                if (extData != null)
                {
                    if (extData.version == 2)
                    {
                        try
                        {
                            Dictionary<int, byte[]> textures = new Dictionary<int, byte[]>();
                            foreach (var dataPair in extData.data.Where(x => x.Key.StartsWith(DataMarker)))
                            {
                                var idStr = dataPair.Key.Substring(DataMarker.Length);
                                if (!int.TryParse(idStr, out var id))
                                    continue;

                                var value = dataPair.Value as byte[];
                                if (value == null && dataPair.Value != null)
                                    continue;

                                textures.Add(id, value);
                            }
                            pd.data.Add("textures", textures);

                            if (extData.data.TryGetValue(LookupDataKey, out var lookup) && lookup is byte[] lookuparr)
                            {
                                var allOverlayTextures = MessagePackSerializer.Deserialize<Dictionary<CoordinateType, Dictionary<TexType, int>>>(lookuparr);
                                pd.data.Add(LookupDataKey, allOverlayTextures);
                            }
                            else
                                pd.data.Add(LookupDataKey, null);

                            return pd;
                        }
                        catch (Exception)
                        { }
                    }
                    return extData;
                }
            }
            else if (extKey == GUID_KCOX)
            {
                var extData = file.GetExtendedDataById(GUID_KCOX);
                if (extData != null)
                {
                    try
                    {
                        pd.version = 1;

                        if (extData.data.TryGetValue(OverlayDataKey, out var overlayData) && overlayData is byte[] overlayBytes)
                        {
                            var deObj = MessagePackSerializer.Deserialize<Dictionary<CoordinateType, Dictionary<string, ClothesTexData>>>(overlayBytes);
                            pd.data.Add(OverlayDataKey, deObj);
                            return pd;
                        }
                    }
                    catch (Exception)
                    { }
                }
                return extData;
            }

            return null;
        }

        public IPlugin.DataState GetDataState(string extKey, PluginData data)
        {
            if (extKey == GUID_KSOX || extKey == GUID_KCOX)
            {
                if (data.data.Count > 0)
                    return IPlugin.DataState.DataPresent;

                return IPlugin.DataState.IsEmpty;
            }
            return IPlugin.DataState.NotSupported;
        }

        private PluginData ReadLegacyData(PluginData data)
        {
            var pd = new PluginData()
            {
                version = data.version
            };

            Dictionary<TexType, byte[]> OverlayStorage = new Dictionary<TexType, byte[]>();
            foreach (TexType texType in Enum.GetValues(typeof(TexType)))
            {
                if (texType == TexType.Unknown) continue;

                if (data != null
                    && data.data.TryGetValue(texType.ToString(), out var texData)
                    && texData is byte[] bytes && bytes.Length > 0)
                {
                    if (texType == TexType.EyeOver)
                    {
                        OverlayStorage.Add(TexType.EyeOverL, bytes);
                        OverlayStorage.Add(TexType.EyeOverR, bytes);
                    }
                    else if (texType == TexType.EyeUnder)
                    {
                        OverlayStorage.Add(TexType.EyeUnderL, bytes);
                        OverlayStorage.Add(TexType.EyeUnderR, bytes);
                    }
                    else
                    {
                        OverlayStorage.Add(texType, bytes);
                    }
                }
            }

            if (OverlayStorage.Count > 0)
            {
                pd.data.Add(LookupDataKey, OverlayStorage);
                return pd;
            }
            return null;
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