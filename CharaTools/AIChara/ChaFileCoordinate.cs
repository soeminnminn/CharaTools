using System;
using System.IO;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    public class ChaFileCoordinate : ChaFileAssist
    {
        [JsonIgnore]
        public static readonly string BlockName = "Coordinate";

        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version loadVersion = new Version(ChaFileDefine.ChaFileCoordinateVersion.ToString());
        
        public string coordinateName = "";
        public int loadProductNo;
        public int language;
        
        public ChaFileClothes clothes;
        public ChaFileAccessory accessory;

        [JsonIgnore]
        public byte[] pngData;
        [JsonIgnore]
        private int lastLoadErrorCode;

        [JsonIgnore]
        public string coordinateFileName { get; internal set; }

        public int GetLastErrorCode()
        {
            return this.lastLoadErrorCode;
        }

        public ChaFileCoordinate()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.clothes = new ChaFileClothes();
            this.accessory = new ChaFileAccessory();
            this.coordinateFileName = "";
            this.coordinateName = "";
            this.pngData = null;
            this.lastLoadErrorCode = 0;
        }

        public byte[] SaveBytes()
        {
            byte[] bufferClothes = MessagePackSerializer.Serialize(this.clothes);
            byte[] bufferAccessory = MessagePackSerializer.Serialize(this.accessory);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(bufferClothes.Length);
                    binaryWriter.Write(bufferClothes);
                    binaryWriter.Write(bufferAccessory.Length);
                    binaryWriter.Write(bufferAccessory);
                    return memoryStream.ToArray();
                }
            }
        }

        public bool LoadBytes(byte[] data, Version ver)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    try
                    {
                        int countClothes = binaryReader.ReadInt32();
                        this.clothes = MessagePackSerializer.Deserialize<ChaFileClothes>(binaryReader.ReadBytes(countClothes));

                        int countAccessory = binaryReader.ReadInt32();
                        this.accessory = MessagePackSerializer.Deserialize<ChaFileAccessory>(binaryReader.ReadBytes(countAccessory));
                    }
                    catch (EndOfStreamException ex)
                    {
                        Debug.LogError(ex);
                        return false;
                    }
                    this.clothes.ComplementWithVersion();
                    this.accessory.ComplementWithVersion();
                    return true;
                }
            }
        }

        [Obsolete]
        public void SaveFile(string path, int lang)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            this.coordinateFileName = Path.GetFileName(path);
            
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    if (this.pngData != null)
                        binaryWriter.Write(this.pngData);
                    binaryWriter.Write(100);
                    binaryWriter.Write("【AIS_Clothes】");
                    binaryWriter.Write(ChaFileDefine.ChaFileClothesVersion.ToString());
                    binaryWriter.Write(lang);
                    binaryWriter.Write(this.coordinateName);
                    byte[] buffer = this.SaveBytes();
                    binaryWriter.Write(buffer.Length);
                    binaryWriter.Write(buffer);
                }
            }
        }

        [Obsolete]
        public bool LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                this.lastLoadErrorCode = -6;
                return false;
            }

            this.coordinateFileName = Path.GetFileName(path);
            
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                return this.LoadFile(fileStream);
        }

        [Obsolete]
        public bool LoadFile(Stream st)
        {
            using (BinaryReader br = new BinaryReader(st))
            {
                try
                {
                    PngAssist.SkipPng(br);
                    if (br.BaseStream.Length - br.BaseStream.Position == 0L)
                    {
                        this.lastLoadErrorCode = -5;
                        return false;
                    }

                    this.loadProductNo = br.ReadInt32();
                    if (this.loadProductNo > 100)
                    {
                        this.lastLoadErrorCode = -3;
                        return false;
                    }
                    
                    if (br.ReadString() != "【AIS_Clothes】")
                    {
                        this.lastLoadErrorCode = -1;
                        return false;
                    }
                    
                    this.loadVersion = new Version(br.ReadString());
                    if (this.loadVersion > ChaFileDefine.ChaFileClothesVersion)
                    {
                        this.lastLoadErrorCode = -2;
                        return false;
                    }
                    
                    this.language = br.ReadInt32();
                    this.coordinateName = br.ReadString();
                    
                    int count = br.ReadInt32();
                    if (this.LoadBytes(br.ReadBytes(count), this.loadVersion))
                    {
                        this.lastLoadErrorCode = 0;
                        return true;
                    }
                    
                    this.lastLoadErrorCode = -999;
                    return false;
                }
                catch (EndOfStreamException ex)
                {
                    Debug.LogError(ex);
                    this.lastLoadErrorCode = -999;
                    return false;
                }
            }
        }

        public void SaveClothes(string path)
        {
            this.SaveFileAssist(path, this.clothes);
        }

        public void LoadClothes(string path)
        {
            this.LoadFileAssist(path, out this.clothes);
            this.clothes.ComplementWithVersion();
        }

        public void SaveAccessory(string path)
        {
            this.SaveFileAssist(path, this.accessory);
        }

        public void LoadAccessory(string path)
        {
            this.LoadFileAssist(path, out this.accessory);
            this.accessory.ComplementWithVersion();
        }
    }
}
