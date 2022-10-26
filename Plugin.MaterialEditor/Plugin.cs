using CharaTools.AIChara;
using CharaTools.Plugin;
using MessagePack;

namespace Plugin.MaterialEditor
{
    public class Plugin : IPlugin
    {
        public const string PluginGUID = "com.deathweasel.bepinex.materialeditor";

        public string GUID => PluginGUID;

        public bool CanDeserialize(string extKey) => extKey == PluginGUID;

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
            var data = file.GetExtendedDataById(PluginGUID);
            if (data != null)
            {
                try
                {
                    var pluginData = new PluginData()
                    {
                        version = data.version
                    };

                    if (data.data.TryGetValue("TextureDictionary", out var texDic) && texDic != null)
                    {
                        var textureDictionary = MessagePackSerializer.Deserialize<Dictionary<int, byte[]>>((byte[])texDic);
                        pluginData.data.Add("TextureDictionary", textureDictionary);
                    }
                    else
                        pluginData.data.Add("TextureDictionary", null);

                    if (data.data.TryGetValue("RendererPropertyList", out var rendererProperties) && rendererProperties != null)
                    {
                        var properties = MessagePackSerializer.Deserialize<List<MaterialEditorCharaController.RendererProperty>>((byte[])rendererProperties);
                        pluginData.data.Add("RendererPropertyList", properties);
                    }
                    else
                        pluginData.data.Add("RendererPropertyList", null);

                    if (data.data.TryGetValue("MaterialFloatPropertyList", out var floatProperties) && floatProperties != null)
                    {
                        var properties = MessagePackSerializer.Deserialize<List<MaterialEditorCharaController.MaterialFloatProperty>>((byte[])floatProperties);
                        pluginData.data.Add("MaterialFloatPropertyList", properties);
                    }
                    else
                        pluginData.data.Add("MaterialFloatPropertyList", null);

                    if (data.data.TryGetValue("MaterialColorPropertyList", out var colorProperties) && colorProperties != null)
                    {
                        var properties = MessagePackSerializer.Deserialize<List<MaterialEditorCharaController.MaterialColorProperty>>((byte[])colorProperties);
                        pluginData.data.Add("MaterialColorPropertyList", properties);
                    }
                    else
                        pluginData.data.Add("MaterialColorPropertyList", null);

                    if (data.data.TryGetValue("MaterialTexturePropertyList", out var textureProperties) && textureProperties != null)
                    {
                        var properties = MessagePackSerializer.Deserialize<List<MaterialEditorCharaController.MaterialTextureProperty>>((byte[])textureProperties);
                        pluginData.data.Add("MaterialTexturePropertyList", properties);
                    }
                    else
                        pluginData.data.Add("MaterialTexturePropertyList", null);

                    if (data.data.TryGetValue("MaterialShaderList", out var shaderProperties) && shaderProperties != null)
                    {
                        var properties = MessagePackSerializer.Deserialize<List<MaterialEditorCharaController.MaterialShader>>((byte[])shaderProperties);
                        pluginData.data.Add("MaterialShaderList", properties);
                    }
                    else
                        pluginData.data.Add("MaterialShaderList", null);

                    if (data.data.TryGetValue("MaterialCopyList", out var copyProperties) && copyProperties != null)
                    {
                        var properties = MessagePackSerializer.Deserialize<List<MaterialEditorCharaController.MaterialCopy>>((byte[])copyProperties);
                        pluginData.data.Add("MaterialCopyList", properties);
                    }
                    else
                        pluginData.data.Add("MaterialCopyList", null);

                    return pluginData;
                }
                catch (Exception)
                { }
            }
            return data;
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