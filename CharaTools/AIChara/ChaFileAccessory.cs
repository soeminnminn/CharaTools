using System;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileAccessory : ExtendedSave
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        public PartsInfo[] parts { get; set; }

        public ChaFileAccessory()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileAccessoryVersion;
            this.parts = new PartsInfo[20];
            for (int index = 0; index < this.parts.Length; ++index)
                this.parts[index] = new PartsInfo();
        }

        public void ComplementWithVersion()
        {
            this.version = ChaFileDefine.ChaFileAccessoryVersion;
        }

        [MessagePackObject(true)]
        public class PartsInfo : ExtendedSave
        {
            public int type { get; set; }

            public int id { get; set; }

            public string parentKey { get; set; }

            public Vector3[,] addMove { get; set; }

            public ColorInfo[] colorInfo { get; set; }

            public int hideCategory { get; set; }

            public int hideTiming { get; set; }

            public bool noShake { get; set; }

            [IgnoreMember]
            public bool partsOfHead { get; set; }

            public PartsInfo()
            {
                this.MemberInit();
            }

            public void MemberInit()
            {
                this.type = 120;
                this.id = 0;
                this.parentKey = "";
                this.addMove = new Vector3[2, 3];
                for (int index = 0; index < 2; ++index)
                {
                    this.addMove[index, 0] = Vector3.zero;
                    this.addMove[index, 1] = Vector3.zero;
                    this.addMove[index, 2] = Vector3.one;
                }
                this.colorInfo = new ColorInfo[4];
                for (int index = 0; index < this.colorInfo.Length; ++index)
                    this.colorInfo[index] = new ColorInfo();
                this.hideCategory = 0;
                this.hideTiming = 1;
                this.partsOfHead = false;
                this.noShake = false;
            }

            [MessagePackObject(true)]
            public class ColorInfo : ExtendedSave
            {
                public Color color { get; set; }

                public float glossPower { get; set; }

                public float metallicPower { get; set; }

                public float smoothnessPower { get; set; }

                public ColorInfo()
                {
                    this.MemberInit();
                }

                public void MemberInit()
                {
                    this.color = Color.white;
                    this.glossPower = 0.5f;
                    this.metallicPower = 0.5f;
                    this.smoothnessPower = 0.5f;
                }
            }
        }
    }
}
