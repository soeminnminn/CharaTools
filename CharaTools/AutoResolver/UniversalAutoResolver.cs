using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CharaTools.AIChara;
using static CharaTools.AIChara.ChaListDefine;

namespace CharaTools.AutoResolver
{
    public class UniversalAutoResolver : IDeserializer
    {
        /// <summary>
        /// Extended save ID
        /// </summary>
        public const string UARExtID = "com.bepis.sideloader.universalautoresolver";
        /// <summary>
        /// Extended save ID used in EmotionCreators once upon a time, no longer used but must still be checked for cards that still use it
        /// </summary>
        public const string UARExtIDOld = "EC.Core.Sideloader.UniversalAutoResolver";

        public static List<ResolveInfo> LoadResolveInfo(ExtendedPlugin file)
        {
            var extData = file.GetExtendedDataById(UARExtIDOld) ?? file.GetExtendedDataById(UARExtID);
            List<ResolveInfo> extInfo = null;

            if (extData == null || !extData.data.ContainsKey("info"))
            {
                Debug.LogError("No sideloader marker found");
                extInfo = null;
            }
            else
            {
                var tmpExtInfo = (object[])extData.data["info"];
                extInfo = tmpExtInfo.Select(x => ResolveInfo.Deserialize((byte[])x)).ToList();

                Debug.Log($"Sideloader marker found, external info count: {extInfo.Count}");

                if (Debug.DebugLogging)
                {
                    foreach (ResolveInfo info in extInfo)
                        Debug.Log($"External info: {info.GUID} : {info.Property} : {info.Slot}");
                }
            }
            return extInfo;
        }

        public static void SaveResolveInfo(ExtendedPlugin file, List<ResolveInfo> resolutionInfo)
        {
            file.SetExtendedDataById(UARExtID, new PluginData
            {
                data = new Dictionary<string, object>
                {
                    ["info"] = resolutionInfo.Select(x => x.Serialize()).ToList()
                }
            });
        }

        private static void CreatePropertiesMap()
        {
            var properties = new List<CategoryProperty>();
            var prefix = string.Empty;

            #region ChaFileFace
            prefix = "ChaFileFace";

            // baseProperties
            properties.Add(new CategoryProperty(CategoryNo.st_eyebrow, "eyebrowId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_eyelash, "eyelashesId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_eye_hl, "hlId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_mole, "moleId", prefix));

            // male
            properties.Add(new CategoryProperty(CategoryNo.mo_head, "headId"));
            properties.Add(new CategoryProperty(CategoryNo.mt_detail_f, "detailId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mt_skin_f, "skinId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mt_beard, "beardId", prefix));

            // female
            properties.Add(new CategoryProperty(CategoryNo.fo_head, "headId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.ft_detail_f, "detailId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.ft_skin_f, "skinId", prefix));

            // generatedProperties
            properties.Add(new CategoryProperty(CategoryNo.st_eye, "Eye1", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_eye, "Eye2", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_eyeblack, "EyeBlack1", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_eyeblack, "EyeBlack2", prefix));
            #endregion

            #region ChaFileBody
            prefix = "ChaFileBody";

            properties.Add(new CategoryProperty(CategoryNo.st_nip, "nipId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_underhair, "underhairId", prefix));

            // male
            properties.Add(new CategoryProperty(CategoryNo.mt_skin_b, "skinId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mt_sunburn, "sunburnId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mt_detail_b, "detailId", prefix));

            // female
            properties.Add(new CategoryProperty(CategoryNo.ft_skin_b, "skinId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.ft_sunburn, "sunburnId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.ft_detail_b, "detailId", prefix));

            properties.Add(new CategoryProperty(CategoryNo.st_paint, "PaintID1", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_paint, "PaintID2", prefix));
            properties.Add(new CategoryProperty(CategoryNo.bodypaint_layout, "PaintLayoutID1", prefix));
            properties.Add(new CategoryProperty(CategoryNo.bodypaint_layout, "PaintLayoutID2", prefix));
            #endregion

            #region ChaFileHair
            prefix = "ChaFileHair";

            properties.Add(new CategoryProperty(CategoryNo.so_hair_b, "HairBack", prefix));
            properties.Add(new CategoryProperty(CategoryNo.so_hair_f, "HairFront", prefix));
            properties.Add(new CategoryProperty(CategoryNo.so_hair_s, "HairSide", prefix));
            properties.Add(new CategoryProperty(CategoryNo.so_hair_o, "HairOption", prefix));

            // hairMeshSupported
            properties.Add(new CategoryProperty(CategoryNo.st_hairmeshptn, "HairBackMesh", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_hairmeshptn, "HairFrontMesh", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_hairmeshptn, "HairSideMesh", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_hairmeshptn, "HairOptionMesh", prefix));
            #endregion

            #region ChaFileFace.MakeupInfo
            prefix = "ChaFileFace.MakeupInfo";

            properties.Add(new CategoryProperty(CategoryNo.st_cheek, "cheekId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_eyeshadow, "eyeshadowId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_lip, "lipId", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_paint, "PaintID1", prefix));
            properties.Add(new CategoryProperty(CategoryNo.st_paint, "PaintID2", prefix));
            properties.Add(new CategoryProperty(CategoryNo.facepaint_layout, "PaintLayoutID1", prefix));
            properties.Add(new CategoryProperty(CategoryNo.facepaint_layout, "PaintLayoutID2", prefix));
            #endregion

            #region ChaFileClothes
            prefix = "ChaFileClothes";

            properties.Add(new CategoryProperty(CategoryNo.fo_top, "ClothesTop", prefix));
            properties.Add(new CategoryProperty(CategoryNo.fo_bot, "ClothesBot", prefix));
            properties.Add(new CategoryProperty(CategoryNo.fo_gloves, "ClothesGloves", prefix));
            properties.Add(new CategoryProperty(CategoryNo.fo_shoes, "ClothesShoes", prefix));

            // male
            properties.Add(new CategoryProperty(CategoryNo.mo_top, "ClothesTopM", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mo_bot, "ClothesBotM", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mo_gloves, "ClothesGlovesM", prefix));
            properties.Add(new CategoryProperty(CategoryNo.mo_shoes, "ClothesShoesM", prefix));

            properties.Add(new CategoryProperty(CategoryNo.fo_inner_t, "ClothesBra", prefix));
            properties.Add(new CategoryProperty(CategoryNo.fo_inner_b, "ClothesShorts", prefix));
            properties.Add(new CategoryProperty(CategoryNo.fo_panst, "ClothesPantyhose", prefix));
            properties.Add(new CategoryProperty(CategoryNo.fo_socks, "ClothesSocks", prefix));

            //Patterns
            for (int index = 0; index < 3; index++)
            {
                // top
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesTopPattern{index}", prefix));
                // bot
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesBotPattern{index}", prefix));
                //bra
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesBraPattern{index}", prefix));
                //shorts
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesShortsPattern{index}", prefix));
                //gloves
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesGlovesPattern{index}", prefix));
                //pants
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesPantyhosePattern{index}", prefix));
                //socks
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesSocksPattern{index}", prefix));
                //shoes
                properties.Add(new CategoryProperty(CategoryNo.st_pattern, $"ClothesShoesPattern{index}", prefix));
            }
            #endregion

            #region ChaFileAccessory.PartsInfo
            prefix = "ChaFileAccessory.PartsInfo";
            var baseProperties = Enum.GetValues(typeof(CategoryNo)).Cast<CategoryNo>().Select(x => new CategoryProperty(x, "id", prefix)).ToList();
            properties.AddRange(baseProperties);
            #endregion
        }

        #region IDeserializer
        public bool CanDeserialize(string extKey) => extKey == UARExtID || extKey == UARExtIDOld;

        public PluginData Serialize(string extKey, PluginData data, ExtendedPlugin file)
        {
            if (extKey == UARExtID || extKey == UARExtIDOld)
            {
                return file.GetExtendedDataById(extKey);
            }
            return data;
        }

        public PluginData Deserialize(string extKey, ExtendedPlugin file)
        {
            var uar = LoadResolveInfo(file);

            var pluginData = new PluginData();
            pluginData.data.Add(extKey, uar);
            
            return pluginData;
        }
        #endregion

        #region Nested types
        internal struct CategoryProperty
        {
            public CategoryNo Category;
            public string Property;

            public string Prefix;

            public CategoryProperty(CategoryNo category, string property, string prefix = "")
            {
                Category = category;
                Property = property;

                Prefix = prefix;
            }

            public override string ToString() => Prefix != "" ? $"{Prefix}.{Property}" : Property;
        }
        #endregion
    }
}
