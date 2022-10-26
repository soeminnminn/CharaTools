using System;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    public class ChaFileControl : ChaFile
    {
        #region Variables
        private const string aisCharaMark = "【AIS_Chara】";
        private const string aisClothesMark = "【AIS_Clothes】";
        private const string kkExMark = "KKEx";

        [JsonIgnore]
        public bool skipRangeCheck = false;

        [JsonIgnore]
        public bool IsAIChara { get; private set; }
        #endregion

        #region Constructor
        public ChaFileControl()
        { }
        #endregion

        #region Properties
        public KKExData KKEx { get; set; } = new KKExData();
        #endregion

        #region Coordinate Methods
        public ChaFileCoordinate LoadCoordinate(Stream stream, bool hasPng = true)
        {
            if (!stream.CanRead || !stream.CanSeek)
                return null;

            using (var br = new BinaryReader(stream))
            {
                if (hasPng) PngAssist.SkipPng(br);

                if (br.BaseStream.Length - br.BaseStream.Position == 0L)
                {
                    this.lastLoadErrorCode = -5;
                    return null;
                }

                try
                {
                    var loadProductNo = br.ReadInt32();
                    if (loadProductNo > 100)
                    {
                        this.lastLoadErrorCode = -3;
                        return null;
                    }

                    var marker = br.ReadString();
                    if (marker != aisClothesMark) 
                    {
                        this.lastLoadErrorCode = -1;
                        return null; 
                    }

                    var coordinate = new ChaFileCoordinate();

                    var ver = new Version(br.ReadString());
                    if (ver > ChaFileDefine.ChaFileClothesVersion)
                    {
                        this.lastLoadErrorCode = -2;
                        return null;
                    }

                    coordinate.loadVersion = ver;
                    coordinate.language = br.ReadInt32();
                    coordinate.coordinateName = br.ReadString();
                    
                    int count = br.ReadInt32();
                    byte[] blockBytes = br.ReadBytes(count);
                    if (!coordinate.LoadBytes(blockBytes, ver))
                    {
                        this.lastLoadErrorCode = -999;
                        return null;
                    }

                    if (br.BaseStream.Length - br.BaseStream.Position == 0L)
                    {
                        this.lastLoadErrorCode = 0;
                        return coordinate;
                    }
                        

                    var exMark = br.ReadString();
                    if (exMark == kkExMark)
                    {
                        var exDataVersion = br.ReadInt32();
                        if (exDataVersion != kkExVersion)
                        {
                            this.lastLoadErrorCode = -2;
                        }
                        else
                        {
                            var exLength = br.ReadInt32();
                            var exBytes = br.ReadBytes(exLength);
                            coordinate.SetExtendedBytes(exBytes, exDataVersion.ToString());
                        }
                    }

                    if (hasPng)
                    {
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        coordinate.pngData = PngAssist.LoadPngData(br);
                    }

                    return coordinate;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    this.lastLoadErrorCode = -999;
                }
            }
            return null;
        }

        public ChaFileCoordinate LoadCoordinate(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                return this.LoadCoordinate(stream, true);
        }

        public bool SaveCoordinate(Stream stream, ChaFileCoordinate coordinate)
        {
            if (stream == null || !stream.CanWrite)
                return false;

            try
            {
                using (var writer = new BinaryWriter(stream))
                {
                    if (coordinate.pngData != null)
                        writer.Write(coordinate.pngData);

                    writer.Write(100);
                    writer.Write(aisClothesMark);
                    writer.Write(ChaFileDefine.ChaFileClothesVersion.ToString());
                    writer.Write(coordinate.language);
                    writer.Write(coordinate.coordinateName);

                    byte[] buffer = coordinate.SaveBytes();
                    writer.Write(buffer.Length);
                    writer.Write(buffer);

                    if (coordinate.ExtendedData != null)
                    {
                        writer.Write(kkExMark);
                        writer.Write(coordinate.kkExVersion);

                        var exBytes = coordinate.GetExtendedBytes();
                        writer.Write(exBytes.Length);
                        writer.Write(exBytes);
                    }

                    writer.Flush();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return false;
        }

        public bool SaveCoordinate(string filePath, ChaFileCoordinate coordinate)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                return SaveCoordinate(fileStream, coordinate);
        }
        #endregion

        #region Chara Methods
        public bool SaveCharaFile(string filename)
        {
            string directoryName = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            this.charaFileName = Path.GetFileName(filename);

            string dataId = this.dataID;

            if (!File.Exists(filename))
                this.dataID = YS_Assist.CreateUUID();

            using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                bool result = SaveCharaFile(fileStream, true);
                this.dataID = dataId;
                return result;
            }
        }

        public bool SaveCharaFile(Stream st, bool savePng)
        {
            using (BinaryWriter bw = new BinaryWriter(st))
                return this.SaveCharaFile(bw, savePng);
        }

        public bool SaveCharaFile(BinaryWriter bw, bool savePng)
        {
            try
            {
                var saveData = BuildCharaDataBlocks();

                if (savePng && pngData != null)
                    bw.Write(pngData);

                bw.Write(100);
                bw.Write(aisCharaMark);
                bw.Write(ChaFileDefine.ChaFileVersion.ToString());
                bw.Write(language);
                bw.Write(userID);
                bw.Write(dataID);

                bw.Write(saveData.InfoData.Length);
                bw.Write(saveData.InfoData);

                bw.Write((long)saveData.Data.Length);
                bw.Write(saveData.Data);

                bw.Flush();

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return false;
        }

        private BlocksData BuildCharaDataBlocks()
        {
            var saveData = new BlocksData();
            var header = new BlockHeader();

            using (var memoryStream = new MemoryStream())
            {
                var lstInfo = new List<BlockHeader.Info>();

                var blockData = GetCustomBytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileCustom.BlockName,
                    version = ChaFileDefine.ChaFileCustomVersion.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                blockData = GetCoordinateBytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileCoordinate.BlockName,
                    version = ChaFileDefine.ChaFileCoordinateVersion.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                blockData = GetParameterBytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileParameter.BlockName,
                    version = ChaFileDefine.ChaFileParameterVersion.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                blockData = GetGameInfoBytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileGameInfo.BlockName,
                    version = ChaFileDefine.ChaFileGameInfoVersion.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                blockData = GetStatusBytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileStatus.BlockName,
                    version = ChaFileDefine.ChaFileStatusVersion.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                blockData = GetParameter2Bytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileParameter2.BlockName,
                    version = ChaFileDefine.ChaFileParameterVersion2.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                blockData = GetGameInfo2Bytes();
                memoryStream.Write(blockData, 0, blockData.Length);
                lstInfo.Add(new BlockHeader.Info()
                {
                    name = ChaFileGameInfo2.BlockName,
                    version = ChaFileDefine.ChaFileGameInfoVersion2.ToString(),
                    pos = memoryStream.Position,
                    size = blockData.Length
                });

                if (ExtendedData != null && ExtendedData.Count > 0)
                {
                    var exData = GetExtendedBytes();
                    memoryStream.Write(exData, 0, exData.Length);

                    var exInfo = new BlockHeader.Info()
                    {
                        name = kkExMark,
                        version = kkExVersion.ToString(),
                        pos = memoryStream.Position,
                        size = exData.Length
                    };
                    header.lstInfo.Add(exInfo);
                }

                saveData.Data = memoryStream.ToArray();

                header.lstInfo.AddRange(lstInfo);
                saveData.InfoData = MessagePackSerializer.Serialize(header);
            }

            return saveData;
        }

        public bool LoadCharaFile(string filename, bool noLoadPng = false, bool noLoadStatus = true)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                this.lastLoadErrorCode = -6;
                return false;
            }

            this.charaFileName = Path.GetFileName(filename);

            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return LoadCharaFile(stream, noLoadPng, noLoadStatus);
        }

        public bool LoadCharaFile(Stream stream, bool noLoadPng = false, bool noLoadStatus = true)
        {
            if (!stream.CanRead || !stream.CanSeek)
                return false;

            using (var br = new BinaryReader(stream))
                return LoadCharaFile(br, noLoadPng, noLoadStatus);
        }

        public bool LoadCharaFile(BinaryReader br, bool noLoadPng = false, bool noLoadStatus = true)
        {
            long pngSize = PngAssist.CheckSize(br);
            if (pngSize != 0L)
            {
                if (noLoadPng)
                    br.BaseStream.Seek(pngSize, SeekOrigin.Current);
                else
                    this.pngData = br.ReadBytes((int)pngSize);

                if (br.BaseStream.Length - br.BaseStream.Position == 0L)
                {
                    this.lastLoadErrorCode = -5;
                    return false;
                }
            }

            try
            {
                var loadProductNo = br.ReadInt32();
                if (loadProductNo > 100)
                {
                    this.lastLoadErrorCode = -3;
                    return false;
                }

                var marker = br.ReadString();
                if (marker != aisCharaMark)
                {
                    this.lastLoadErrorCode = -1;
                    return false;
                }

                var loadVersion = new Version(br.ReadString());
                if (loadVersion > ChaFileDefine.ChaFileVersion)
                {
                    this.lastLoadErrorCode = -2;
                    return false;
                }

                this.loadProductNo = loadProductNo;
                this.loadVersion = loadVersion;

                this.language = br.ReadInt32();
                this.userID = br.ReadString();
                this.dataID = br.ReadString();

                var headerSize = br.ReadInt32();
                var headerBytes = br.ReadBytes(headerSize);

                var blockHeader = MessagePackSerializer.Deserialize<BlockHeader>(headerBytes);
                if (blockHeader != null)
                {
                    var dataSize = br.ReadInt64();
                    var position = br.BaseStream.Position;

                    foreach (var info in blockHeader.lstInfo)
                    {
                        var seekPos = br.BaseStream.Seek(position + info.pos, SeekOrigin.Begin);
                        if (seekPos < br.BaseStream.Length)
                        {
                            var blockBytes = br.ReadBytes((int)info.size);
                            if (blockBytes != null && blockBytes.Length > 0)
                            {
                                if (info.name == ChaFileCustom.BlockName)
                                {
                                    Version ver = new Version(info.version);
                                    if (ver > ChaFileDefine.ChaFileCustomVersion)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetCustomBytes(blockBytes, ver);
                                    continue;
                                }
                                else if (info.name == ChaFileCoordinate.BlockName)
                                {
                                    Version ver = new Version(info.version);
                                    if (ver > ChaFileDefine.ChaFileCoordinateVersion)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetCoordinateBytes(blockBytes, ver);
                                    continue;
                                }
                                else if (info.name == ChaFileParameter.BlockName)
                                {
                                    if (new Version(info.version) > ChaFileDefine.ChaFileParameterVersion)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetParameterBytes(blockBytes);
                                    continue;
                                }
                                else if (info.name == ChaFileParameter2.BlockName)
                                {
                                    if (new Version(info.version) > ChaFileDefine.ChaFileParameterVersion2)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetParameter2Bytes(blockBytes);
                                    continue;
                                }
                                else if (info.name == ChaFileGameInfo.BlockName)
                                {
                                    if (new Version(info.version) > ChaFileDefine.ChaFileGameInfoVersion)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetGameInfoBytes(blockBytes);
                                    continue;
                                }
                                else if (info.name == ChaFileGameInfo2.BlockName)
                                {
                                    if (new Version(info.version) > ChaFileDefine.ChaFileGameInfoVersion2)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetGameInfo2Bytes(blockBytes);
                                    continue;
                                }
                                else if (!noLoadStatus && info.name == ChaFileStatus.BlockName)
                                {
                                    if (new Version(info.version) > ChaFileDefine.ChaFileStatusVersion)
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetStatusBytes(blockBytes);
                                    continue;
                                }
                                else if (info.name == kkExMark)
                                {
                                    if (info.version != kkExVersion.ToString())
                                        this.lastLoadErrorCode = -2;
                                    else
                                        SetExtendedBytes(blockBytes, info.version);
                                    continue;
                                }

                                // var data = MessagePackSerializer.Typeless.Deserialize(blockBytes);
                                // var blockJson = MessagePackSerializer.ConvertToJson(blockBytes);
                                // System.Diagnostics.Trace.WriteLine($"\"{info.name}\": {blockJson}");
                            }
                        }
                    }

                    var gi2 = blockHeader.SearchInfo(ChaFileGameInfo2.BlockName);
                    var p2 = blockHeader.SearchInfo(ChaFileParameter2.BlockName);
                    IsAIChara = gi2 == null && p2 == null;
                }

                this.lastLoadErrorCode = 0;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                this.lastLoadErrorCode = -999;
            }
            return false;
        }

        public static string GenerateFileName(string game, byte sex)
        {
            string fileName = game;
            fileName += sex == 0 ? "ChaM_" : "ChaF_";
            fileName += DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
            return fileName;
        }
        #endregion

        #region Nested Types
        private class BlocksData
        {
            public byte[] InfoData { get; internal set; }
            public byte[] Data { get; internal set; }
        }

        public class KKExData
        {
            public int Version { get; set; }
            public Dictionary<string, PluginData> Data { get; set; } = new Dictionary<string, PluginData>();
        }
        #endregion
    }
}
