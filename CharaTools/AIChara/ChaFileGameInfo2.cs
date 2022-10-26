using System;
using System.Collections.Generic;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    [MessagePackObject(true)]
    public class ChaFileGameInfo2 : ExtendedSave
    {
        [JsonIgnore]
        [IgnoreMember]
        public static readonly string BlockName = "GameInfo2";

        public bool[][] genericVoice = new bool[2][];
        public bool[] inviteVoice = new bool[5];
        
        public HashSet<int> map = new HashSet<int>();
        
        private int favor;
        private int enjoyment;
        private int aversion;
        private int slavery;
        private int broken;
        private int dependence;
        
        public ChaFileDefine.State nowState;
        public ChaFileDefine.State nowDrawState;
        
        public bool lockNowState;
        public bool lockBroken;
        public bool lockDependence;
        
        private int dirty;
        private int tiredness;
        private int toilet;
        private int libido;

        public int alertness;
        
        public ChaFileDefine.State calcState;
        
        public byte escapeFlag;
        public bool escapeExperienced;
        
        public bool firstHFlag;
        
        public bool genericBrokenVoice;
        public bool genericDependencepVoice;
        public bool genericAnalVoice;
        public bool genericPainVoice;
        public bool genericFlag;
        public bool genericBefore;
        
        public int hCount;
        
        public bool arriveRoom50;
        public bool arriveRoom80;
        public bool arriveRoomHAfter;
        
        public int resistH;
        public int resistPain;
        public int resistAnal;
        
        public int usedItem;

        public bool isChangeParameter;
        public bool isConcierge;

        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version version { get; set; }

        public int Favor
        {
            get
            {
                return this.favor;
            }
            set
            {
                this.favor = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Enjoyment
        {
            get
            {
                return this.enjoyment;
            }
            set
            {
                this.enjoyment = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Aversion
        {
            get
            {
                return this.aversion;
            }
            set
            {
                this.aversion = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Slavery
        {
            get
            {
                return this.slavery;
            }
            set
            {
                this.slavery = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Broken
        {
            get
            {
                return this.broken;
            }
            set
            {
                this.broken = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Dependence
        {
            get
            {
                return this.dependence;
            }
            set
            {
                this.dependence = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Dirty
        {
            get
            {
                return this.dirty;
            }
            set
            {
                this.dirty = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Tiredness
        {
            get
            {
                return this.tiredness;
            }
            set
            {
                this.tiredness = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Toilet
        {
            get
            {
                return this.toilet;
            }
            set
            {
                this.toilet = Mathf.Clamp(value, 0, 100);
            }
        }

        public int Libido
        {
            get
            {
                return this.libido;
            }
            set
            {
                this.libido = Mathf.Clamp(value, 0, 100);
            }
        }

        public ChaFileGameInfo2()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.version = ChaFileDefine.ChaFileGameInfoVersion;
            this.favor = 0;
            this.enjoyment = 0;
            this.aversion = 0;
            this.slavery = 0;
            this.broken = 0;
            this.dependence = 0;
            this.nowState = ChaFileDefine.State.Blank;
            this.nowDrawState = ChaFileDefine.State.Blank;
            this.lockNowState = false;
            this.lockBroken = false;
            this.lockDependence = false;
            this.dirty = 0;
            this.tiredness = 0;
            this.toilet = 0;
            this.libido = 0;
            this.alertness = 0;
            this.calcState = ChaFileDefine.State.Blank;
            this.escapeFlag = 0;
            this.escapeExperienced = false;
            this.firstHFlag = false;
            this.genericVoice = new bool[2][];
            this.genericVoice[0] = new bool[13];
            this.genericVoice[1] = new bool[13];
            this.genericBrokenVoice = false;
            this.genericDependencepVoice = false;
            this.genericAnalVoice = false;
            this.genericPainVoice = false;
            this.genericFlag = false;
            this.genericBefore = false;
            this.inviteVoice = new bool[5];
            this.hCount = 0;
            this.map = new HashSet<int>();
            this.arriveRoom50 = false;
            this.arriveRoom80 = false;
            this.arriveRoomHAfter = false;
            this.resistH = 0;
            this.resistPain = 0;
            this.resistAnal = 0;
            this.usedItem = 0;
            this.isChangeParameter = false;
            this.isConcierge = false;
        }

        public void Copy(ChaFileGameInfo2 src)
        {
            this.version = src.version;
            this.favor = src.favor;
            this.enjoyment = src.enjoyment;
            this.aversion = src.aversion;
            this.slavery = src.slavery;
            this.broken = src.broken;
            this.dependence = src.dependence;
            this.nowState = src.nowState;
            this.nowDrawState = src.nowDrawState;
            this.lockNowState = src.lockNowState;
            this.lockBroken = src.lockBroken;
            this.lockDependence = src.lockDependence;
            this.dirty = src.dirty;
            this.tiredness = src.tiredness;
            this.toilet = src.toilet;
            this.libido = src.libido;
            this.alertness = src.alertness;
            this.calcState = src.calcState;
            this.escapeFlag = src.escapeFlag;
            this.escapeExperienced = src.escapeExperienced;
            this.firstHFlag = src.firstHFlag;
            Array.Copy(src.genericVoice, genericVoice, src.genericVoice.Length);
            this.genericBrokenVoice = src.genericBrokenVoice;
            this.genericDependencepVoice = src.genericDependencepVoice;
            this.genericAnalVoice = src.genericAnalVoice;
            this.genericPainVoice = src.genericPainVoice;
            this.genericFlag = src.genericFlag;
            this.genericBefore = src.genericBefore;
            Array.Copy(src.inviteVoice, inviteVoice, src.inviteVoice.Length);
            this.hCount = src.hCount;
            this.map = new HashSet<int>(src.map);
            this.arriveRoom50 = src.arriveRoom50;
            this.arriveRoom80 = src.arriveRoom80;
            this.arriveRoomHAfter = src.arriveRoomHAfter;
            this.resistH = src.resistH;
            this.resistPain = src.resistPain;
            this.resistAnal = src.resistAnal;
            this.usedItem = src.usedItem;
            this.isChangeParameter = src.isChangeParameter;
            this.isConcierge = src.isConcierge;
        }

        public void ComplementWithVersion()
        {
            this.version = ChaFileDefine.ChaFileGameInfoVersion;
        }
    }
}
