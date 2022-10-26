using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using CharaTools.AIChara;
using CharaTools.Observable;
using Microsoft.Win32;

namespace CharaTools
{
    public class CharacterEditor : ObservableObject
    {
        #region Variables
        private ChaFile file = null;
        private bool hasChanged = false;
        private bool freeze = false;

        private IDeserializer[] deserializers = new IDeserializer[]
        {
            new AutoResolver.UniversalAutoResolver(),
            new MoreAccessories()
        };
        #endregion

        #region Constructor
        public CharacterEditor()
        {
        }
        #endregion

        #region Properties
        public ChaFile Card
        {
            get => file;
            set
            {
                if (value is ChaFileControl && value.GetLastErrorCode() == 0)
                {
                    file = value;
                    KKEx = ((ChaFileControl)value).KKEx;
                }
                else if (value.GetLastErrorCode() == 0)
                {
                    file = value;
                    KKEx = new ChaFileControl.KKExData();
                }

                OnPropertyChanged(nameof(IsEmpty));
                LoadData();
            }
        }

        public bool IsAIChara { get; private set; }

        public bool IsEmpty
        {
            get => file == null;
        }

        public bool HasChanged 
        { 
            get => hasChanged;
            private set { SetProperty(ref hasChanged, value); }
        }

        public string CharaFilePath { get; private set; }

        public string CharaFileName { get; private set; }

        public byte[] PngData { get; private set; }

        #region ChaFileParameter
        private string fullname = string.Empty;
        public string FullName 
        {
            get => fullname;
            set { SetProperty(ref fullname, value); } 
        }

        private byte sex = 0;
        public byte Sex 
        { 
            get => sex;
            set { SetProperty(ref sex, value); }
        }

        private byte birthDay = 0;
        public byte BirthDay 
        {
            get => birthDay;
            set { SetProperty(ref birthDay, value); }
        }

        private byte birthMonth = 0;
        public byte BirthMonth 
        {
            get => birthMonth;
            set { SetProperty(ref birthMonth, value); }
        }

        private int personality = 0;
        public int Personality 
        {
            get => personality;
            set { SetProperty(ref personality, value); }
        }

        private float voiceRate = 0.0f;
        public float VoiceRate 
        {
            get => voiceRate;
            set { SetProperty(ref voiceRate, value); }
        }

        private byte trait = 0;
        public byte Trait 
        {
            get => trait;
            set { SetProperty(ref trait, value); }
        }

        private byte mind = 0;
        public byte Mind 
        {
            get => mind;
            set { SetProperty(ref mind, value); }
        }

        private byte hAttribute = 0;
        public byte HAttribute 
        {
            get => hAttribute;
            set { SetProperty(ref hAttribute, value); }
        }
        #endregion

        #region ChaFileGameInfo
        private bool gameRegistration = false;
        public bool GameRegistration
        {
            get => gameRegistration;
            set { SetProperty(ref gameRegistration, value); }
        }

        private int favoritePlace = -1;
        public int FavoritePlace 
        {
            get => favoritePlace;
            set { SetProperty(ref favoritePlace, value); }
        }

        private int lifeStyle = -1;
        public int LifeStyle
        {
            get => lifeStyle;
            set { SetProperty(ref lifeStyle, value); }
        }

        private int morality = 0;
        public int Morality
        {
            get => morality;
            set { SetProperty(ref morality, value); }
        }

        private int motivation = 0;
        public int Motivation
        {
            get => motivation;
            set { SetProperty(ref motivation, value); }
        }

        private int immoral = 0;
        public int Immoral
        {
            get => immoral;
            set { SetProperty(ref immoral, value); }
        }

        private bool isHAddTaii0 = false;
        public bool IsHAddTaii0
        {
            get => isHAddTaii0;
            set { SetProperty(ref isHAddTaii0, value); }
        }

        private bool isHAddTaii1 = false;
        public bool IsHAddTaii1
        {
            get => isHAddTaii1;
            set { SetProperty(ref isHAddTaii1, value); }
        }

        private int aversion = 0;
        public int Aversion
        {
            get => aversion;
            set { SetProperty(ref aversion, value); }
        }

        private int enjoyment = 0;
        public int Enjoyment
        {
            get => enjoyment;
            set { SetProperty(ref enjoyment, value); }
        }

        private int favor = 0;
        public int Favor
        {
            get => favor;
            set { SetProperty(ref favor, value); }
        }

        private int slavery = 0;
        public int Slavery
        {
            get => slavery;
            set { SetProperty(ref slavery, value); }
        }

        private int broken = 0;
        public int Broken
        {
            get => broken;
            set { SetProperty(ref broken, value); }
        }

        private int dependence = 0;
        public int Dependence
        {
            get => dependence;
            set { SetProperty(ref dependence, value); }
        }

        private int dirty = 0;
        public int Dirty
        {
            get => dirty;
            set { SetProperty(ref dirty, value); }
        }

        private int tiredness = 0;
        public int Tiredness
        {
            get => tiredness;
            set { SetProperty(ref tiredness, value); }
        }

        private int toilet = 0;
        public int Toilet
        {
            get => toilet;
            set { SetProperty(ref toilet, value); }
        }

        private int libido = 0;
        public int Libido
        {
            get => libido;
            set { SetProperty(ref libido, value); }
        }

        private bool firstHFlag = false;
        public bool FirstHFlag 
        {
            get => firstHFlag;
            set { SetProperty(ref firstHFlag, value); } 
        }

        private int hCount = 0;
        public int HCount
        {
            get => hCount;
            set { SetProperty(ref hCount, value); }
        }

        private int alertness = 0;
        public int Alertness
        {
            get => alertness;
            set { SetProperty(ref alertness, value); }
        }

        private int usedItem = 0;
        public int UsedItem
        {
            get => usedItem;
            set { SetProperty(ref usedItem, value); }
        }

        private int resistH = 0;
        public int ResistH
        {
            get => resistH;
            set { SetProperty(ref resistH, value); }
        }

        private int resistPain = 0;
        public int ResistPain
        {
            get => resistPain;
            set { SetProperty(ref resistPain, value); }
        }

        private int resistAnal = 0;
        public int ResistAnal
        {
            get => resistAnal;
            set { SetProperty(ref resistAnal, value); }
        }

        private ChaFileDefine.State nowState = ChaFileDefine.State.Blank;
        public ChaFileDefine.State NowState 
        {
            get => nowState;
            set { SetProperty(ref nowState, value); }
        }

        private ChaFileDefine.State nowDrawState = ChaFileDefine.State.Blank;
        public ChaFileDefine.State NowDrawState
        {
            get => nowDrawState;
            set { SetProperty(ref nowDrawState, value); }
        }
        #endregion

        private ChaFileCoordinate coordinate = null;
        public ChaFileCoordinate Coordinate 
        {
            get => coordinate;
            private set { SetProperty(ref coordinate, value); }
        }

        private Dictionary<string, PluginData> extendedData = null;
        public Dictionary<string, PluginData> ExtendedData 
        {
            get => extendedData;
            private set { SetProperty(ref extendedData, value); } 
        }

        private ChaFileControl.KKExData kkEx = null;
        public ChaFileControl.KKExData KKEx 
        {
            get => kkEx;
            private set { SetProperty(ref kkEx, value); } 
        }

        public CoordEditor CoordinateEditor { get; private set; } = null;
        #endregion

        #region Methods

        #region Load / Fill
        private void LoadData()
        {
            freeze = true;

            ResetProperties();

            if (file != null)
            {
                CharaFileName = file.charaFileName;
                PngData = file.pngData;

                Coordinate = file.coordinate;
                ExtendedData = file.ExtendedData;

                LoadChaFileParameter(file.parameter);
                LoadChaFileParameter2(file.parameter2);
                LoadChaFileGameInfo(file.gameinfo);
                LoadChaFileGameInfo2(file.gameinfo2);

                CoordinateEditor = new CoordEditor(Coordinate, file.status, KKEx == null ? null : KKEx.Data);
            }

            HasChanged = false;
            freeze = false;
        }

        private void FillData(ChaFile chaFile)
        {
            FillChaFileGameInfo(chaFile.gameinfo);
            FillChaFileGameInfo2(chaFile.gameinfo2);

            FillChaFileParameter(chaFile.parameter);
            FillChaFileParameter2(chaFile.parameter2);
        }

        private void LoadChaFileParameter(ChaFileParameter _param)
        {
            if (_param == null) return;

            FullName = _param.fullname;
            Sex = _param.sex;

            BirthDay = _param.birthDay;
            BirthMonth = _param.birthMonth;

            Personality = _param.personality;
            VoiceRate = _param.voiceRate;
        }

        private void FillChaFileParameter(ChaFileParameter _param)
        {
            _param.fullname = FullName;
            _param.sex = Sex;

            _param.birthDay = BirthDay;
            _param.birthMonth = BirthMonth;

            _param.personality = Personality;
            _param.voiceRate = VoiceRate;
        }

        private void LoadChaFileParameter2(ChaFileParameter2 _param)
        {
            if (_param == null) return;

            Personality = _param.personality;
            Trait = _param.trait;
            Mind = _param.mind;
            HAttribute = _param.hAttribute;
            VoiceRate = _param.voiceRate;
        }

        private void FillChaFileParameter2(ChaFileParameter2 _param)
        {
            _param.personality = Personality;
            _param.trait = Trait;
            _param.mind = Mind;
            _param.hAttribute = HAttribute;
            _param.voiceRate = VoiceRate;
        }

        private void LoadChaFileGameInfo(ChaFileGameInfo info)
        {
            if (info == null) return;

            GameRegistration = info.gameRegistration;

            FavoritePlace = info.favoritePlace;
            LifeStyle = info.lifestyle;
            Morality = info.morality;
            Motivation = info.motivation;
            Immoral = info.immoral;

            IsHAddTaii0 = info.isHAddTaii0;
            IsHAddTaii1 = info.isHAddTaii1;
        }

        private void FillChaFileGameInfo(ChaFileGameInfo info)
        {
            info.gameRegistration = GameRegistration;

            info.favoritePlace = FavoritePlace;
            info.lifestyle = LifeStyle;
            info.morality = Morality;
            info.motivation = Motivation;
            info.immoral = Immoral;

            info.isHAddTaii0 = IsHAddTaii0;
            info.isHAddTaii1 = IsHAddTaii1;
        }

        private void LoadChaFileGameInfo2(ChaFileGameInfo2 info)
        {
            if (info == null) return;

            Aversion = info.Aversion;
            Enjoyment = info.Enjoyment;
            Favor = info.Favor;
            Slavery = info.Slavery;
            Broken = info.Broken;
            Dependence = info.Dependence;

            Dirty = info.Dirty;
            Tiredness = info.Tiredness;
            Toilet = info.Toilet;
            Libido = info.Libido;

            FirstHFlag = info.firstHFlag;
            HCount = info.hCount;

            Alertness = info.alertness;
            UsedItem = info.usedItem;

            ResistH = info.resistH;
            ResistPain = info.resistPain;
            ResistAnal = info.resistAnal;

            NowState = info.nowState;
            NowDrawState = info.nowDrawState;
        }

        private void FillChaFileGameInfo2(ChaFileGameInfo2 info)
        {
            info.Aversion = Aversion;
            info.Enjoyment = Enjoyment;
            info.Favor = Favor;
            info.Slavery = Slavery;
            info.Broken = Broken;
            info.Dependence = Dependence;

            info.Dirty = Dirty;
            info.Tiredness = Tiredness;
            info.Toilet = Toilet;
            info.Libido = Libido;

            info.firstHFlag = FirstHFlag;
            info.hCount = HCount;

            info.alertness = Alertness;
            info.usedItem = UsedItem;

            info.resistH = ResistH;
            info.resistPain = ResistPain;
            info.resistAnal = ResistAnal;

            info.nowState = NowState;
            info.nowDrawState = NowDrawState;
        }

        public bool Load(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                ChaFileControl chaFileCard = new ChaFileControl();
                try
                {
                    chaFileCard.LoadCharaFile(filePath, false, false);

                    if (chaFileCard.ExtendedData != null && chaFileCard.ExtendedData.Count > 0)
                    {
                        chaFileCard.KKEx = new ChaFileControl.KKExData();
                        chaFileCard.KKEx.Version = chaFileCard.kkExVersion;

                        foreach (var ExtKey in chaFileCard.ExtendedData.Keys)
                        {
                            PluginData pluginData = null;

                            var plugin = Plugin.Manager.GetDeserializer(ExtKey);
                            if (plugin != null)
                                pluginData = plugin.Deserialize(ExtKey, chaFileCard);

                            if (pluginData == null)
                            {
                                foreach (IDeserializer deserializer in deserializers)
                                {
                                    if (deserializer.CanDeserialize(ExtKey))
                                    {
                                        pluginData = deserializer.Deserialize(ExtKey, chaFileCard);
                                        break;
                                    }
                                }
                            }

                            if (pluginData == null)
                                pluginData = chaFileCard.GetExtendedDataById(ExtKey);

                            chaFileCard.KKEx.Data.Add(ExtKey, pluginData);
                        }
                    }

                    if (chaFileCard.GetLastErrorCode() == 0)
                    {
                        Card = chaFileCard;
                        IsAIChara = chaFileCard.IsAIChara;
                        CharaFilePath = filePath;
                        return true;
                    }
                }
                catch (Exception err)
                {
                    Debug.LogError(err);
                }
            }
            return false;
        }
        #endregion

        #region Reset
        public void Reset()
        {
            file = null;
            ResetProperties();
        }

        private void ResetProperties()
        {
            FullName = string.Empty;
            BirthDay = 1;
            BirthMonth = 1;
            Personality = 0;
            VoiceRate = 0.5f;
            Trait = 0;
            Mind = 0;
            HAttribute = 0;

            FavoritePlace = -1;
            LifeStyle = -1;
            Morality = 50;
            Motivation = 0;
            Immoral = 0;
            IsHAddTaii0 = false;
            IsHAddTaii1 = false;

            Enjoyment = 0;
            Favor = 0;
            Aversion = 0;
            Slavery = 0;
            Broken = 0;
            Dependence = 0;
            NowState = ChaFileDefine.State.Blank;
            NowDrawState = ChaFileDefine.State.Blank;
            Dirty = 0;
            Tiredness = 0;
            Toilet = 0;
            Libido = 0;
            Alertness = 0;
            FirstHFlag = false;
            HCount = 0;
            ResistH = 0;
            ResistPain = 0;
            ResistAnal = 0;
            UsedItem = 0;

            hasChanged = false;
        }
        #endregion

        public string SerializeJson(bool excludeExtData = false, bool isIndented = true)
        {
            if (IsEmpty) return string.Empty;

            try
            {
                var settings = new JsonSerializerSettings();

                if (isIndented)
                    settings.Formatting = Formatting.Indented;

                settings.ContractResolver = new Json.ChaFileContractResolver()
                {
                    IsAIChara = IsAIChara,
                    ExcludeExtendedData = excludeExtData
                };

                var card = new ChaFileControl();

                Card.CopyAll(card);
                FillData(card);

                if (CoordinateEditor != null)
                    CoordinateEditor.FillData(card);
                
                if (!excludeExtData)
                {
                    if (CoordinateEditor != null)
                        card.KKEx = CoordinateEditor.FillData(KKEx);
                    else
                        card.KKEx = KKEx;
                }

                return JsonConvert.SerializeObject(card, settings);
            }
            catch (Exception)
            { }

            return string.Empty;
        }

        private void CalcState(string propertyName = null)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var checkProperties = new List<string> 
                {
                    nameof(Personality), nameof(Favor),
                    nameof(Enjoyment), nameof(Slavery),
                    nameof(Aversion)
                };
                if (checkProperties.IndexOf(propertyName) == -1) return;
            }

            ChaFileGameInfo2 info = new ChaFileGameInfo2();
            FillChaFileGameInfo2(info);
            HS2.GlobalHS2Calc.CalcState(info, Personality);
            LoadChaFileGameInfo2(info);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {            
            base.OnPropertyChanged(propertyName);
            if (freeze) return;

            if (propertyName != nameof(HasChanged) && propertyName != nameof(IsEmpty))
            {
                HasChanged = true;
                CalcState(propertyName);
            }
        }

        public string GenerateFileName()
        {
            string game = IsAIChara ? "AIS" : "HS2";
            return ChaFileControl.GenerateFileName(game, Sex);
        }

        public bool Save(string fileName = null, byte[] pngData = null, bool skipEmptyExt = false)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = CharaFileName;
            if (string.IsNullOrEmpty(fileName))
                fileName = GenerateFileName();
            if (!fileName.ToLowerInvariant().EndsWith(".png"))
                fileName += ".png";

            var card = new ChaFileControl();

            Card.CopyAll(card);
            FillData(card);

            if (pngData != null)
                card.pngData = pngData;
            else
                card.pngData = PngData;

            card.ExtendedData = ExtendedData;
            BuildExtendedData(card, skipEmptyExt);

            Plugin.Manager.SendOnChaFileBeforeSave(card);

            return card.SaveCharaFile(fileName);
        }

        private void BuildExtendedData(ChaFileControl card, bool skipEmptyExt)
        {
            if (CoordinateEditor != null)
                CoordinateEditor.FillData(card);

            foreach (var extKey in KKEx.Data.Keys)
            {
                var pluginData = KKEx.Data[extKey];
                PluginData serializedData = null;
                Plugin.IPlugin.DataState dataState = Plugin.IPlugin.DataState.NotSupported;

                foreach (IDeserializer deserializer in deserializers)
                {
                    if (deserializer.CanDeserialize(extKey))
                    {
                        serializedData = deserializer.Serialize(extKey, pluginData, card);
                        dataState = Plugin.IPlugin.DataState.DataPresent;
                        card.SetExtendedDataById(extKey, serializedData);
                        break;
                    }
                }

                if (serializedData == null && dataState == Plugin.IPlugin.DataState.NotSupported)
                {
                    var plugin = Plugin.Manager.GetDeserializer(extKey);
                    if (plugin != null)
                    {
                        dataState = plugin.GetDataState(extKey, pluginData);

                        if (dataState == Plugin.IPlugin.DataState.DataPresent)
                        {
                            serializedData = plugin.Serialize(extKey, pluginData, card);
                            card.SetExtendedDataById(extKey, serializedData);
                        }
                        else if (skipEmptyExt && dataState == Plugin.IPlugin.DataState.IsEmpty && card.ExtendedData.ContainsKey(extKey))
                            card.ExtendedData.Remove(extKey);
                    }

                    if (skipEmptyExt && dataState == Plugin.IPlugin.DataState.NotSupported && card.ExtendedData.ContainsKey(extKey))
                    {
                        serializedData = card.GetExtendedDataById(extKey);
                        if (serializedData == null || serializedData.data == null || serializedData.data.Count == 0)
                            card.ExtendedData.Remove(extKey);
                    }
                }
            }
        }
        #endregion
    }
}
