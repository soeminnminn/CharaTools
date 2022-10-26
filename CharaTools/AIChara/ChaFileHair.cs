using System;
using System.Collections.Generic;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileHair : ExtendedSave
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        public bool sameSetting { get; set; }

        public bool autoSetting { get; set; }

        public bool ctrlTogether { get; set; }

        public PartsInfo[] parts { get; set; }

        public int kind { get; set; }

        public int shaderType { get; set; }

        public ChaFileHair()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileHairVersion;
            this.sameSetting = true;
            this.autoSetting = true;
            this.ctrlTogether = false;
            this.parts = new PartsInfo[Enum.GetValues(typeof(ChaFileDefine.HairKind)).Length];
            for (int index = 0; index < this.parts.Length; ++index)
                this.parts[index] = new PartsInfo();
            this.kind = 0;
            this.shaderType = 0;
        }

        public void ComplementWithVersion()
        {
            if (this.version < new Version("0.0.1"))
            {
                for (int index1 = 0; index1 < this.parts.Length; ++index1)
                {
                    this.parts[index1].acsColorInfo = new PartsInfo.ColorInfo[4];
                    for (int index2 = 0; index2 < this.parts[index1].acsColorInfo.Length; ++index2)
                        this.parts[index1].acsColorInfo[index2] = new PartsInfo.ColorInfo();
                }
            }

            if (this.version < new Version("0.0.2"))
            {
                this.sameSetting = true;
                this.autoSetting = true;
                this.ctrlTogether = false;
            }

            if (this.version < new Version("0.0.3"))
            {
                PartsInfo.BundleInfo bundleInfo;
                if (4 == this.parts[0].id)
                {
                    if (this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
                        bundleInfo.rotRate = new Vector3(bundleInfo.rotRate.x, 1f - bundleInfo.rotRate.y, bundleInfo.rotRate.z);
                }
                else if (5 == this.parts[0].id)
                {
                    if (this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
                    {
                        float z = Mathf.InverseLerp(-30f, 30f, Mathf.Lerp(3f, 30f, bundleInfo.rotRate.z));
                        bundleInfo.rotRate = new Vector3(bundleInfo.rotRate.x, bundleInfo.rotRate.y, z);
                    }

                    if (this.parts[0].dictBundle.TryGetValue(1, out bundleInfo))
                    {
                        float z = Mathf.InverseLerp(0.1f, -0.4f, Mathf.Lerp(0.1f, -0.1f, bundleInfo.moveRate.z));
                        bundleInfo.moveRate = new Vector3(bundleInfo.moveRate.x, bundleInfo.moveRate.y, z);
                    }

                    if (this.parts[0].dictBundle.TryGetValue(2, out bundleInfo))
                    {
                        float x = Mathf.InverseLerp(-25f, 50f, Mathf.Lerp(-25f, 45f, bundleInfo.rotRate.x));
                        bundleInfo.rotRate = new Vector3(x, bundleInfo.rotRate.y, bundleInfo.rotRate.z);
                    }

                    if (this.parts[0].dictBundle.TryGetValue(3, out bundleInfo))
                    {
                        float z = Mathf.InverseLerp(-0.1f, 0.4f, Mathf.Lerp(-0.1f, -0.4f, bundleInfo.moveRate.z));
                        bundleInfo.moveRate = new Vector3(bundleInfo.moveRate.x, bundleInfo.moveRate.y, z);
                        
                        float x = Mathf.InverseLerp(45f, -22.5f, Mathf.Lerp(-22.5f, 45f, bundleInfo.rotRate.x));
                        bundleInfo.rotRate = new Vector3(x, bundleInfo.rotRate.y, bundleInfo.rotRate.z);
                    }

                    if (this.parts[0].dictBundle.TryGetValue(4, out bundleInfo))
                    {
                        float x = Mathf.InverseLerp(45f, -22.5f, Mathf.Lerp(-22.5f, 45f, bundleInfo.rotRate.x));
                        bundleInfo.rotRate = new Vector3(x, bundleInfo.rotRate.y, bundleInfo.rotRate.z);
                    }
                }
                else if (8 == this.parts[0].id && this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
                    bundleInfo.rotRate = new Vector3(bundleInfo.rotRate.x, 1f - bundleInfo.rotRate.y, bundleInfo.rotRate.z);
                
                if (7 == this.parts[1].id && this.parts[0].dictBundle.TryGetValue(0, out bundleInfo))
                {
                    float y = Mathf.InverseLerp(70f, -35f, Mathf.Lerp(-70f, 35f, bundleInfo.rotRate.y));
                    bundleInfo.rotRate = new Vector3(bundleInfo.rotRate.x, y, bundleInfo.rotRate.z);
                }
            }
            this.version = ChaFileDefine.ChaFileHairVersion;
        }

        [MessagePackObject(true)]
        public class PartsInfo : ExtendedSave
        {
            public int id { get; set; }

            public Color baseColor { get; set; }

            public Color topColor { get; set; }

            public Color underColor { get; set; }

            public Color specular { get; set; }

            public float metallic { get; set; }

            public float smoothness { get; set; }

            public ColorInfo[] acsColorInfo { get; set; }

            public int bundleId { get; set; }

            public Dictionary<int, BundleInfo> dictBundle { get; set; }

            public int meshType { get; set; }

            public Color meshColor { get; set; }

            public Vector4 meshLayout { get; set; }

            public PartsInfo()
            {
                this.MemberInit();
            }

            public void MemberInit()
            {
                this.id = 0;
                this.baseColor = new Color(0.2f, 0.2f, 0.2f);
                this.topColor = new Color(0.039f, 0.039f, 0.039f);
                this.underColor = new Color(0.565f, 0.565f, 0.565f);
                this.specular = new Color(0.3f, 0.3f, 0.3f);
                this.metallic = 0.0f;
                this.smoothness = 0.0f;
                this.acsColorInfo = new ColorInfo[4];
                for (int index = 0; index < this.acsColorInfo.Length; ++index)
                    this.acsColorInfo[index] = new ColorInfo();
                this.bundleId = -1;
                this.dictBundle = new Dictionary<int, BundleInfo>();
                this.meshType = 0;
                this.meshColor = new Color(1f, 1f, 1f, 1f);
                this.meshLayout = new Vector4(1f, 1f, 0.0f, 0.0f);
            }

            [MessagePackObject(true)]
            public class BundleInfo : ExtendedSave
            {
                public Vector3 moveRate { get; set; }

                public Vector3 rotRate { get; set; }

                public bool noShake { get; set; }

                public BundleInfo()
                {
                    this.MemberInit();
                }

                public void MemberInit()
                {
                    this.moveRate = Vector3.zero;
                    this.rotRate = Vector3.zero;
                    this.noShake = false;
                }
            }

            [MessagePackObject(true)]
            public class ColorInfo : ExtendedSave
            {
                public Color color { get; set; }

                public ColorInfo()
                {
                    this.MemberInit();
                }

                public void MemberInit()
                {
                    this.color = Color.white;
                }
            }
        }
    }
}
