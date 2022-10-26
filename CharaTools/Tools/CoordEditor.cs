using System;
using System.Collections.Generic;
using System.Linq;
using CharaTools.AIChara;
using CharaTools.AutoResolver;
using Newtonsoft.Json;
using static CharaTools.AIChara.ChaFileDefine;

namespace CharaTools
{
    public class CoordEditor
    {
        #region Variables
        private static readonly string[] ClothesProps = new string[]
        {
            "ClothesTop", "ClothesBot", "ClothesBra", "ClothesShorts", "ClothesGloves",
            "ClothesPantyhose", "ClothesSocks", "ClothesShoes"
        };

        public Dictionary<ClothesKind, ClothesInfo> clothes = new Dictionary<ClothesKind, ClothesInfo>();

        public List<AccessoryInfo> accessories = new List<AccessoryInfo>();
        #endregion

        #region Contructor
        public CoordEditor(ChaFileControl file)
        {
            BuildParts(file.coordinate, file.status, file.KKEx == null ? null : file.KKEx.Data);
        }

        public CoordEditor(ChaFileCoordinate coordinate, ChaFileStatus status, Dictionary<string, PluginData> extData)
        {
            BuildParts(coordinate, status, extData);
        }
        #endregion

        #region Methods
        private void BuildParts(ChaFileCoordinate coordinate, ChaFileStatus status, Dictionary<string, PluginData> extData)
        {
            if (coordinate != null)
            {
                List<ChaFileClothes.PartsInfo> clothesParts = new List<ChaFileClothes.PartsInfo>();
                List<ChaFileAccessory.PartsInfo> accessoryParts = new List<ChaFileAccessory.PartsInfo>();

                if (coordinate.clothes != null && coordinate.clothes.parts != null)
                    clothesParts.AddRange(coordinate.clothes.parts);

                if (coordinate.accessory != null && coordinate.accessory.parts != null)
                    accessoryParts.AddRange(coordinate.accessory.parts);

                MoreAccessories.AdditionalData moreAccs = null;
                List<ResolveInfo> listInfo = null;

                if (extData == null)
                {
                    moreAccs = MoreAccessories.LoadAdditionalData(coordinate);
                    if (moreAccs != null && moreAccs.parts.Count > 0)
                        accessoryParts.AddRange(moreAccs.parts);

                    listInfo = UniversalAutoResolver.LoadResolveInfo(coordinate);
                }
                else
                {
                    if (extData.TryGetValue(MoreAccessories._extSaveKey, out PluginData moreAccsData))
                    {
                        if (moreAccsData.data.TryGetValue(MoreAccessories._extDataKey, out object obj) && obj is MoreAccessories.AdditionalData)
                        {
                            moreAccs = (MoreAccessories.AdditionalData)obj;
                            accessoryParts.AddRange(moreAccs.parts);
                        }
                    }

                    PluginData data;
                    if (!extData.TryGetValue(UniversalAutoResolver.UARExtID, out data))
                        extData.TryGetValue(UniversalAutoResolver.UARExtIDOld, out data);

                    if (data != null)
                    {
                        if (data.data.TryGetValue(UniversalAutoResolver.UARExtID, out object obj) && obj is List<ResolveInfo>)
                            listInfo = (List<ResolveInfo>)obj;
                        else if (data.data.TryGetValue(UniversalAutoResolver.UARExtIDOld, out object objOld) && objOld is List<ResolveInfo>)
                            listInfo = (List<ResolveInfo>)objOld;
                    }
                }

                byte[] clothesState = status != null ? status.clothesState : new byte[Enum.GetValues(typeof(ClothesKind)).Length];
                bool[] showAccessory = status != null ? status.showAccessory : new bool[20];

                MapResolveInfo(clothesParts, accessoryParts, clothesState, showAccessory, listInfo, moreAccs);
            }
        }

        private void MapResolveInfo(
            List<ChaFileClothes.PartsInfo> clothesParts, 
            List<ChaFileAccessory.PartsInfo> accessoryParts,
            byte[] clothesState, bool[] showAccessory,
            List<ResolveInfo> listInfo, MoreAccessories.AdditionalData moreAccs)
        {
            if (clothesParts.Count > 0 && clothesParts.Count == ClothesProps.Length)
            {
                for (int i = 0; i < clothesParts.Count; i++)
                {
                    var kind = (ClothesKind)i;
                    var cInfo = new ClothesInfo()
                    {
                        PartsInfo = clothesParts[i],
                        State = clothesState.Length > i ? clothesState[i] : (byte)0
                    };

                    if (listInfo != null && listInfo.Count > 0)
                    {
                        var propName = $"outfit.ChaFileClothes.{ClothesProps[i]}";
                        cInfo.UARInfo = listInfo.SingleOrDefault(x => x.Property == propName);

                        propName += "Pattern";
                        cInfo.Patterns = listInfo.FindAll(x => x.Property.StartsWith(propName));
                    }

                    clothes.Add(kind, cInfo);
                }
            }

            if (accessoryParts.Count > 0)
            {
                for (int i = 0; i < accessoryParts.Count; i++)
                {
                    var accInfo = new AccessoryInfo()
                    {
                        PartsInfo = accessoryParts[i]
                    };

                    if (showAccessory.Length > i)
                        accInfo.Show = showAccessory[i];
                    else if (i >= 20 && moreAccs != null)
                    {
                        var m = i - 20;
                        if (moreAccs.objects != null && moreAccs.objects.Count > m)
                            accInfo.Show = moreAccs.objects[m].show;
                    }

                    if (listInfo != null && listInfo.Count > 0)
                    {
                        var propName = $"outfit.accessory{i}.ChaFileAccessory.PartsInfo.id";
                        accInfo.UARInfo = listInfo.SingleOrDefault(x => x.Property == propName);
                    }
                    
                    accessories.Add(accInfo);
                }
            }
        }

        public void FillData(ChaFileControl file)
        {
            //
        }

        public ChaFileControl.KKExData FillData(ChaFileControl.KKExData kkEx)
        {
            //
            return kkEx;
        }
        #endregion

        #region Nested types
        public class ClothesInfo
        {
            public byte State { get; set; }

            [JsonProperty("PartInfo")]
            public ChaFileClothes.PartsInfo PartsInfo { get; set; }

            [JsonProperty("UniversalAutoResolver")]
            public ResolveInfo UARInfo { get; set; }

            public List<ResolveInfo> Patterns { get; set; } = new List<ResolveInfo>();
        }

        public class AccessoryInfo
        {
            public bool Show { get; set; } = true;

            [JsonProperty("PartInfo")]
            public ChaFileAccessory.PartsInfo PartsInfo { get; set; }

            [JsonProperty("UniversalAutoResolver")]
            public ResolveInfo UARInfo { get; set; }
        }
        #endregion
    }
}
