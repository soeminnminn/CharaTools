using System;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileClothes : ExtendedSave
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        public PartsInfo[] parts { get; set; }

        public ChaFileClothes()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileClothesVersion;
            this.parts = new PartsInfo[Enum.GetValues(typeof(ChaFileDefine.ClothesKind)).Length];
            for (int index = 0; index < this.parts.Length; ++index)
                this.parts[index] = new PartsInfo();
        }

        public void ComplementWithVersion()
        {
            this.version = ChaFileDefine.ChaFileClothesVersion;
        }

        [MessagePackObject(true)]
        public class PartsInfo : ExtendedSave
        {
            public int id { get; set; }

            public ColorInfo[] colorInfo { get; set; }

            public float breakRate { get; set; }

            public bool[] hideOpt { get; set; }

            public PartsInfo()
            {
                this.MemberInit();
            }

            public void MemberInit()
            {
                this.id = 0;
                this.colorInfo = new ColorInfo[3];
                for (int index = 0; index < this.colorInfo.Length; ++index)
                    this.colorInfo[index] = new ColorInfo();
                this.breakRate = 0.0f;
                this.hideOpt = new bool[2];
            }

            [MessagePackObject(true)]
            public class ColorInfo : ExtendedSave
            {
                public Color baseColor { get; set; }

                public int pattern { get; set; }

                public Vector4 layout { get; set; }

                public float rotation { get; set; }

                public Color patternColor { get; set; }

                public float glossPower { get; set; }

                public float metallicPower { get; set; }

                public ColorInfo()
                {
                    this.MemberInit();
                }

                public void MemberInit()
                {
                    this.baseColor = Color.white;
                    this.pattern = 0;
                    this.layout = new Vector4(1f, 1f, 0.0f, 0.0f);
                    this.rotation = 0.5f;
                    this.patternColor = Color.white;
                    this.glossPower = 0.5f;
                    this.metallicPower = 0.0f;
                }
            }
        }
    }
}
