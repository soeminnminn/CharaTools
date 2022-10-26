using System;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileParameter2 : ExtendedSave
    {
        [JsonIgnore]
        [IgnoreMember]
        public static readonly string BlockName = "Parameter2";

        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        [JsonConverter(typeof(Json.ParameterIntConverter))]
        public int personality { get; set; }

        public float voiceRate { get; set; }

        [JsonIgnore]
        [IgnoreMember]
        public float voicePitch
        {
            get
            {
                return Mathf.Lerp(0.94f, 1.06f, this.voiceRate);
            }
        }

        [JsonConverter(typeof(Json.ParameterByteConverter))]
        public byte trait { get; set; }

        [JsonConverter(typeof(Json.ParameterByteConverter))]
        public byte mind { get; set; }

        [JsonConverter(typeof(Json.ParameterByteConverter))]
        public byte hAttribute { get; set; }

        public ChaFileParameter2()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileParameterVersion2;
            this.personality = 0;
            this.voiceRate = 0.5f;
            this.trait = 0;
            this.mind = 0;
            this.hAttribute = 0;
        }

        public void Copy(ChaFileParameter2 src)
        {
            this.version = src.version;
            this.personality = src.personality;
            this.voiceRate = src.voiceRate;
            this.trait = src.trait;
            this.mind = src.mind;
            this.hAttribute = src.hAttribute;
        }

        public void ComplementWithVersion()
        {
            this.version = ChaFileDefine.ChaFileParameterVersion2;
        }
    }
}
