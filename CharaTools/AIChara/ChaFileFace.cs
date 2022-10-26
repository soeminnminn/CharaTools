using System;
using System.Collections.Generic;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileFace : ExtendedSave
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        public float[] shapeValueFace { get; set; }

        public int headId { get; set; }

        public int skinId { get; set; }

        public int detailId { get; set; }

        public float detailPower { get; set; }

        public int eyebrowId { get; set; }

        public Color eyebrowColor { get; set; }

        public Vector4 eyebrowLayout { get; set; }

        public float eyebrowTilt { get; set; }

        public EyesInfo[] pupil { get; set; }

        public bool pupilSameSetting { get; set; }

        public float pupilY { get; set; }

        public int hlId { get; set; }

        public Color hlColor { get; set; }

        public Vector4 hlLayout { get; set; }

        public float hlTilt { get; set; }

        public float whiteShadowScale { get; set; }

        public int eyelashesId { get; set; }

        public Color eyelashesColor { get; set; }

        public int moleId { get; set; }

        public Color moleColor { get; set; }

        public Vector4 moleLayout { get; set; }

        public MakeupInfo makeup { get; set; }

        public int beardId { get; set; }

        public Color beardColor { get; set; }

        public ChaFileFace()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileFaceVersion;
            this.shapeValueFace = new float[ChaFileDefine.cf_headshapename.Length];
            for (int index = 0; index < this.shapeValueFace.Length; ++index)
                this.shapeValueFace[index] = ChaFileDefine.cf_faceInitValue[index];
            this.headId = 0;
            this.skinId = 0;
            this.detailId = 0;
            this.detailPower = 1f;
            this.eyebrowId = 0;
            this.eyebrowColor = new Color(0.05f, 0.05f, 0.05f);
            this.eyebrowLayout = new Vector4(0.5f, 0.375f, 0.666f, 0.666f);
            this.eyebrowTilt = 0.5f;
            this.pupil = new EyesInfo[2];
            for (int index = 0; index < this.pupil.Length; ++index)
                this.pupil[index] = new EyesInfo();
            this.pupilSameSetting = true;
            this.pupilY = 0.5f;
            this.hlId = 0;
            this.hlColor = Color.white;
            this.hlLayout = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
            this.hlTilt = 0.5f;
            this.whiteShadowScale = 0.5f;
            this.eyelashesId = 0;
            this.eyelashesColor = new Color(0.19f, 0.19f, 0.19f);
            this.moleId = 0;
            this.moleColor = Color.black;
            this.moleLayout = new Vector4(0.5f, 0.5f, 0.25f, 0.5f);
            this.makeup = new MakeupInfo();
            this.beardId = 0;
            this.beardColor = new Color(0.19f, 0.19f, 0.19f);
        }

        public void ComplementWithVersion()
        {
            if (this.version < new Version("0.0.1"))
            {
                this.hlLayout = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
                this.hlTilt = 0.5f;
            }

            if (this.version < new Version("0.0.2"))
            {
                this.beardId = 0;
                this.beardColor = new Color(0.19f, 0.19f, 0.19f);
            }

            this.version = ChaFileDefine.ChaFileFaceVersion;
        }

        [MessagePackObject(true)]
        public class EyesInfo : ExtendedSave
        {
            public Color whiteColor { get; set; }

            public int pupilId { get; set; }

            public Color pupilColor { get; set; }

            public float pupilW { get; set; }

            public float pupilH { get; set; }

            public float pupilEmission { get; set; }

            public int blackId { get; set; }

            public Color blackColor { get; set; }

            public float blackW { get; set; }

            public float blackH { get; set; }

            public EyesInfo()
            {
                this.MemberInit();
            }

            public void MemberInit()
            {
                this.whiteColor = new Color(0.846f, 0.846f, 0.846f);
                this.pupilId = 0;
                this.pupilColor = new Color(0.33f, 0.33f, 0.33f);
                this.pupilW = 0.666f;
                this.pupilH = 0.666f;
                this.pupilEmission = 0.0f;
                this.blackId = 0;
                this.blackColor = Color.black;
                this.blackW = 0.8333f;
                this.blackH = 0.8333f;
            }

            public void Copy(EyesInfo src)
            {
                this.whiteColor = src.whiteColor;
                this.pupilId = src.pupilId;
                this.pupilColor = src.pupilColor;
                this.pupilW = src.pupilW;
                this.pupilH = src.pupilH;
                this.pupilEmission = src.pupilEmission;
                this.blackId = src.blackId;
                this.blackColor = src.blackColor;
                this.blackW = src.blackW;
                this.blackH = src.blackH;
            }
        }

        [MessagePackObject(true)]
        public class MakeupInfo : ExtendedSave
        {
            public int eyeshadowId { get; set; }

            public Color eyeshadowColor { get; set; }

            public float eyeshadowGloss { get; set; }

            public int cheekId { get; set; }

            public Color cheekColor { get; set; }

            public float cheekGloss { get; set; }

            public int lipId { get; set; }

            public Color lipColor { get; set; }

            public float lipGloss { get; set; }

            public PaintInfo[] paintInfo { get; set; }

            public MakeupInfo()
            {
                this.MemberInit();
            }

            public void MemberInit()
            {
                this.eyeshadowId = 0;
                this.eyeshadowColor = Color.white;
                this.cheekId = 0;
                this.cheekColor = Color.white;
                this.lipId = 0;
                this.lipColor = Color.white;
                this.paintInfo = new PaintInfo[2];
                for (int index = 0; index < 2; ++index)
                    this.paintInfo[index] = new PaintInfo();
            }
        }
    }
}
