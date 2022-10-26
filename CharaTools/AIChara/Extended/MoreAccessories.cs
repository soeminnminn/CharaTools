using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CharaTools.AIChara
{
    public class MoreAccessories : IDeserializer
    {
        internal const string _name = "MoreAccessories";
        internal const string _version = "1.2.2";
        internal const string _guid = "com.joan6694.illusionplugins.moreaccessories";
        internal const string _extSaveKey = "moreAccessories";
        internal const string _extDataKey = "additionalAccessories";
        internal const int _saveVersion = 0;

        public static AdditionalData LoadAdditionalData(ExtendedPlugin file)
        {
            PluginData pluginData = file.GetExtendedDataById(_extSaveKey);
            AdditionalData additionalData = new AdditionalData();

            XmlNode node = null;
            if (pluginData != null && pluginData.data.TryGetValue(_extDataKey, out object xmlData))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml((string)xmlData);
                node = doc.FirstChild;
            }

            if (node != null)
                LoadAdditionalData(ref additionalData, node);

            return additionalData;
        }

        private static void LoadAdditionalData(ref AdditionalData data, XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "accessory":
                        ChaFileAccessory.PartsInfo part = new ChaFileAccessory.PartsInfo();
                        part.type = XmlConvert.ToInt32(childNode.Attributes["type"].Value);
                        if (part.type != 350)
                        {
                            part.id = XmlConvert.ToInt32(childNode.Attributes["id"].Value);
                            part.parentKey = childNode.Attributes["parentKey"].Value;

                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    part.addMove[i, j] = new Vector3
                                    {
                                        x = XmlConvert.ToSingle(childNode.Attributes[$"addMove{i}{j}x"].Value),
                                        y = XmlConvert.ToSingle(childNode.Attributes[$"addMove{i}{j}y"].Value),
                                        z = XmlConvert.ToSingle(childNode.Attributes[$"addMove{i}{j}z"].Value)
                                    };
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                part.colorInfo[i] = new ChaFileAccessory.PartsInfo.ColorInfo()
                                {
                                    color = new Color()
                                    {
                                        r = XmlConvert.ToSingle(childNode.Attributes[$"color{i}r"].Value),
                                        g = XmlConvert.ToSingle(childNode.Attributes[$"color{i}g"].Value),
                                        b = XmlConvert.ToSingle(childNode.Attributes[$"color{i}b"].Value),
                                        a = XmlConvert.ToSingle(childNode.Attributes[$"color{i}a"].Value)
                                    },
                                    glossPower = XmlConvert.ToSingle(childNode.Attributes[$"glossPower{i}"].Value),
                                    metallicPower = XmlConvert.ToSingle(childNode.Attributes[$"metallicPower{i}"].Value),
                                    smoothnessPower = XmlConvert.ToSingle(childNode.Attributes[$"smoothnessPower{i}"].Value)
                                };
                            }
                            part.hideCategory = XmlConvert.ToInt32(childNode.Attributes["hideCategory"].Value);
                            part.hideTiming = XmlConvert.ToInt32(childNode.Attributes["hideTiming"].Value);
                            part.noShake = XmlConvert.ToBoolean(childNode.Attributes["noShake"].Value);

                        }
                        data.parts.Add(part);
                        data.objects.Add(new AdditionalData.AccessoryObject());
                        if (childNode.Attributes["show"] != null)
                            data.objects[data.objects.Count - 1].show = XmlConvert.ToBoolean(childNode.Attributes["show"].Value);
                        break;
                }
            }
        }

        public static void SaveAdditionalData(AdditionalData data, XmlWriter xmlWriter, bool isStudio = false)
        {
            xmlWriter.WriteStartElement("additionalAccessories");
            xmlWriter.WriteAttributeString("version", _version);
            for (int index = 0; index < data.parts.Count; index++)
            {
                ChaFileAccessory.PartsInfo part = data.parts[index];
                xmlWriter.WriteStartElement("accessory");
                xmlWriter.WriteAttributeString("type", XmlConvert.ToString(part.type));

                if (part.type != 350)
                {
                    xmlWriter.WriteAttributeString("id", XmlConvert.ToString(part.id));
                    xmlWriter.WriteAttributeString("parentKey", part.parentKey);

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Vector3 v = part.addMove[i, j];
                            xmlWriter.WriteAttributeString($"addMove{i}{j}x", XmlConvert.ToString(v.x));
                            xmlWriter.WriteAttributeString($"addMove{i}{j}y", XmlConvert.ToString(v.y));
                            xmlWriter.WriteAttributeString($"addMove{i}{j}z", XmlConvert.ToString(v.z));
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        ChaFileAccessory.PartsInfo.ColorInfo colorInfo = part.colorInfo[i];
                        xmlWriter.WriteAttributeString($"color{i}r", XmlConvert.ToString(colorInfo.color.r));
                        xmlWriter.WriteAttributeString($"color{i}g", XmlConvert.ToString(colorInfo.color.g));
                        xmlWriter.WriteAttributeString($"color{i}b", XmlConvert.ToString(colorInfo.color.b));
                        xmlWriter.WriteAttributeString($"color{i}a", XmlConvert.ToString(colorInfo.color.a));

                        xmlWriter.WriteAttributeString($"glossPower{i}", XmlConvert.ToString(colorInfo.glossPower));
                        xmlWriter.WriteAttributeString($"metallicPower{i}", XmlConvert.ToString(colorInfo.metallicPower));
                        xmlWriter.WriteAttributeString($"smoothnessPower{i}", XmlConvert.ToString(colorInfo.smoothnessPower));
                    }

                    xmlWriter.WriteAttributeString("hideCategory", XmlConvert.ToString(part.hideCategory));
                    xmlWriter.WriteAttributeString("hideTiming", XmlConvert.ToString(part.hideCategory));
                    xmlWriter.WriteAttributeString("noShake", XmlConvert.ToString(part.noShake));
                    if (isStudio)
                        xmlWriter.WriteAttributeString("show", XmlConvert.ToString(data.objects[index].show));
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
        }

        public static void SaveAdditionalData(ExtendedPlugin file, AdditionalData data, bool isStudio = false)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {
                SaveAdditionalData(data, xmlWriter, isStudio);

                PluginData pluginData = new PluginData();
                pluginData.version = _saveVersion;
                pluginData.data.Add("additionalAccessories", stringWriter.ToString());

                file.SetExtendedDataById(_extSaveKey, pluginData);
            }
        }

        #region IDeserializer
        public bool CanDeserialize(string extKey) => extKey == _extSaveKey;

        public PluginData Serialize(string extKey, PluginData data, ExtendedPlugin file)
        {
            if (extKey == _extSaveKey)
            {
                return file.GetExtendedDataById(extKey);
            }
            return data;
        }

        public PluginData Deserialize(string extKey, ExtendedPlugin file)
        {
            var moreAccs = LoadAdditionalData(file);
            var pluginData = new PluginData()
            {
                version = _saveVersion
            };
            pluginData.data.Add(_extDataKey, moreAccs);
            return pluginData;
        }
        #endregion

        #region Nested types
        public class AdditionalData
        {
            public List<ChaFileAccessory.PartsInfo> parts = new List<ChaFileAccessory.PartsInfo>();
            public List<AccessoryObject> objects = new List<AccessoryObject>();

            public class AccessoryObject
            {
                public bool show = true;
            }
        }
        #endregion
    }
}
