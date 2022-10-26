using System;
using System.Windows.Documents;
using System.Windows.Markup;

namespace CharaTools.Models
{
    public class CoordPartListData
    {
        #region Properties
        public string PartType { get; set; } = string.Empty;

        public int Index { get; set; }

        public string Kind { get; set; } = string.Empty;

        public string ID { get; set; } = string.Empty;

        public string ParentKey { get; set; } = string.Empty;

        public string Moded { get; set; } = string.Empty;

        public string DetailsJson { get; private set; } = string.Empty;
        #endregion

        #region Methods
        public static CoordPartListData From(AIChara.ChaFileDefine.ClothesKind kind, AIChara.ChaFileClothes.PartsInfo part, bool moded = false, string json = null)
        {
            return new CoordPartListData()
            {
                PartType = "Clothes",
                Index = ((int)kind) + 1,
                Kind = kind.ToString(),
                ID = part == null ? "[Empty]" : part.id.ToString(),
                Moded = part == null ? "" : moded ? "Yes" : "No",
                DetailsJson = json == null ? string.Empty : json
            };
        }

        public static CoordPartListData From(int index, AIChara.ChaFileAccessory.PartsInfo part, bool moded = false, string json = null)
        {
            var data = new CoordPartListData()
            {
                PartType = "Accessories",
                Index = index + 1,
            };

            if (part == null)
            {
                data.Kind = "[Empty]";
            }
            else
            {
                var aoType = (AIChara.ChaListDefine.CategoryNo)part.type;
                data.Kind = aoType.ToString();
                data.ID = part.id.ToString();
                data.ParentKey = part.parentKey;
                data.Moded = moded ? "Yes" : "No";
                data.DetailsJson = json == null ? string.Empty : json;
            }

            return data;
        }
        #endregion
    }
}
