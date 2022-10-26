using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileParameter : ExtendedSave
    {
        [JsonIgnore]
        [IgnoreMember]
        public static readonly string BlockName = "Parameter";

        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        public byte sex { get; set; }

        public string fullname { get; set; }

        [JsonConverter(typeof(Json.ParameterIntConverter))]
        public int personality { get; set; }

        public byte birthMonth { get; set; }

        public byte birthDay { get; set; }

        [JsonIgnore]
        [IgnoreMember]
        public string strBirthDay
        {
            get
            {
                return ChaFileDefine.GetBirthdayStr(this.birthMonth, this.birthDay);
            }
        }

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

        public HashSet<int> hsWish { get; set; }

        [JsonIgnore]
        [IgnoreMember]
        public int wish01
        {
            get
            {
                return this.hsWish.Count == 0 ? -1 : this.hsWish.ToArray()[0];
            }
        }

        [JsonIgnore]
        [IgnoreMember]
        public int wish02
        {
            get
            {
                return 1 >= this.hsWish.Count ? -1 : this.hsWish.ToArray()[1];
            }
        }

        [JsonIgnore]
        [IgnoreMember]
        public int wish03
        {
            get
            {
                return 2 >= this.hsWish.Count ? -1 : this.hsWish.ToArray()[2];
            }
        }

        public bool futanari { get; set; }

        public ChaFileParameter()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileParameterVersion;
            this.sex = 0;
            this.fullname = "";
            this.personality = 0;
            this.birthMonth = 1;
            this.birthDay = 1;
            this.voiceRate = 0.5f;
            this.hsWish = new HashSet<int>();
            this.futanari = false;
        }

        public void Copy(ChaFileParameter src)
        {
            this.version = src.version;
            this.sex = src.sex;
            this.fullname = src.fullname;
            this.personality = src.personality;
            this.birthMonth = src.birthMonth;
            this.birthDay = src.birthDay;
            this.voiceRate = src.voiceRate;
            this.hsWish = new HashSet<int>(src.hsWish);
            this.futanari = src.futanari;
        }

        public void ComplementWithVersion()
        {
            if (this.version < new Version("0.0.1"))
                this.hsWish = new HashSet<int>();
            this.version = ChaFileDefine.ChaFileParameterVersion;
        }
    }
}
