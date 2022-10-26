using System;
using System.IO;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    public class ChaFile : ExtendedPlugin
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version loadVersion = new Version(ChaFileDefine.ChaFileVersion.ToString());
        
        public string userID = "";
        public string dataID = "";
        public int loadProductNo;
        public int language;

        [JsonIgnore]
        public byte[] pngData;

        public ChaFileCustom custom;
        public ChaFileCoordinate coordinate;
        public ChaFileParameter parameter;
        public ChaFileGameInfo gameinfo;
        public ChaFileParameter2 parameter2;
        public ChaFileGameInfo2 gameinfo2;
        public ChaFileStatus status;

        [JsonIgnore]
        public ChaFileCoordinateBath coordinateBath;
        [JsonIgnore]
        public ChaFileCoordinatePajamas coordinatePajamas;

        [JsonIgnore]
        protected int lastLoadErrorCode;

        [JsonIgnore]
        public string charaFileName { get; protected set; }

        public int GetLastErrorCode()
        {
            return this.lastLoadErrorCode;
        }

        public ChaFile()
        {
            this.custom = new ChaFileCustom();
            this.coordinate = new ChaFileCoordinate();
            this.parameter = new ChaFileParameter();
            this.gameinfo = new ChaFileGameInfo();
            this.parameter2 = new ChaFileParameter2();
            this.gameinfo2 = new ChaFileGameInfo2();
            this.status = new ChaFileStatus();
            this.coordinateBath = new ChaFileCoordinateBath();
            this.coordinatePajamas = new ChaFileCoordinatePajamas();
            this.lastLoadErrorCode = 0;
        }

        [Obsolete]
        public bool SaveFile(string path, int lang)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            this.charaFileName = Path.GetFileName(path);
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                return this.SaveFile(fileStream, true, lang);
        }

        [Obsolete]
        public bool SaveFile(Stream st, bool savePng, int lang)
        {
            using (BinaryWriter bw = new BinaryWriter(st))
                return this.SaveFile(bw, savePng, lang);
        }

        [Obsolete]
        public bool SaveFile(BinaryWriter bw, bool savePng, int lang)
        {
            if (savePng && this.pngData != null)
                bw.Write(this.pngData);
            bw.Write(100);
            bw.Write("【AIS_Chara】");
            bw.Write(ChaFileDefine.ChaFileVersion.ToString());
            bw.Write(lang);
            bw.Write(this.userID);
            bw.Write(this.dataID);
            
            byte[] customBytes = this.GetCustomBytes();
            byte[] coordinateBytes = this.GetCoordinateBytes();
            byte[] parameterBytes = this.GetParameterBytes();
            byte[] gameInfoBytes = this.GetGameInfoBytes();
            byte[] statusBytes = this.GetStatusBytes();
            byte[] parameter2Bytes = this.GetParameter2Bytes();
            byte[] gameInfo2Bytes = this.GetGameInfo2Bytes();

            int length = 7;
            long pos = 0;
            string[] blockNameArr = new string[7]
            {
                ChaFileCustom.BlockName,
                ChaFileCoordinate.BlockName,
                ChaFileParameter.BlockName,
                ChaFileGameInfo.BlockName,
                ChaFileStatus.BlockName,
                ChaFileParameter2.BlockName,
                ChaFileGameInfo2.BlockName
            };
            
            string[] versionArr = new string[7]
            {
                ChaFileDefine.ChaFileCustomVersion.ToString(),
                ChaFileDefine.ChaFileCoordinateVersion.ToString(),
                ChaFileDefine.ChaFileParameterVersion.ToString(),
                ChaFileDefine.ChaFileGameInfoVersion.ToString(),
                ChaFileDefine.ChaFileStatusVersion.ToString(),
                ChaFileDefine.ChaFileParameterVersion2.ToString(),
                ChaFileDefine.ChaFileGameInfoVersion2.ToString()
            };
            
            long[] lengthArr = new long[length];
            lengthArr[0] = customBytes == null ? 0L : customBytes.Length;
            lengthArr[1] = coordinateBytes == null ? 0L : coordinateBytes.Length;
            lengthArr[2] = parameterBytes == null ? 0L : parameterBytes.Length;
            lengthArr[3] = gameInfoBytes == null ? 0L : gameInfoBytes.Length;
            lengthArr[4] = statusBytes == null ? 0L : statusBytes.Length;
            lengthArr[5] = parameter2Bytes == null ? 0L : parameter2Bytes.Length;
            lengthArr[6] = gameInfo2Bytes == null ? 0L : gameInfo2Bytes.Length;
            
            long[] posArr = new long[7]
            {
                pos,
                pos + lengthArr[0],
                pos + lengthArr[0] + lengthArr[1],
                pos + lengthArr[0] + lengthArr[1] + lengthArr[2],
                pos + lengthArr[0] + lengthArr[1] + lengthArr[2] + lengthArr[3],
                pos + lengthArr[0] + lengthArr[1] + lengthArr[2] + lengthArr[3] + lengthArr[4],
                pos + lengthArr[0] + lengthArr[1] + lengthArr[2] + lengthArr[3] + lengthArr[4] + lengthArr[5]
            };
            
            BlockHeader blockHeader = new BlockHeader();
            for (int index = 0; index < length; ++index)
            {
                BlockHeader.Info info = new BlockHeader.Info()
                {
                    name = blockNameArr[index],
                    version = versionArr[index],
                    size = lengthArr[index],
                    pos = posArr[index]
                };
                blockHeader.lstInfo.Add(info);
            }
            
            byte[] buffer = MessagePackSerializer.Serialize(blockHeader);
            bw.Write(buffer.Length);
            bw.Write(buffer);
            
            long dataLength = 0;
            foreach (long len in lengthArr)
                dataLength += len;
            
            bw.Write(dataLength);
            bw.Write(customBytes);
            bw.Write(coordinateBytes);
            bw.Write(parameterBytes);
            bw.Write(gameInfoBytes);
            bw.Write(statusBytes);
            bw.Write(parameter2Bytes);
            bw.Write(gameInfo2Bytes);
            return true;
        }

        [Obsolete]
        public bool LoadFile(string path, int lang, bool noLoadPNG = false, bool noLoadStatus = true)
        {
            if (!File.Exists(path))
            {
                this.lastLoadErrorCode = -6;
                return false;
            }

            this.charaFileName = Path.GetFileName(path);
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                return this.LoadFile(fileStream, lang, noLoadPNG, noLoadStatus);
        }

        [Obsolete]
        public bool LoadFile(Stream st, int lang, bool noLoadPNG = false, bool noLoadStatus = true)
        {
            using (BinaryReader br = new BinaryReader(st))
                return this.LoadFile(br, lang, noLoadPNG, noLoadStatus);
        }

        [Obsolete]
        public bool LoadFile(BinaryReader br, int lang, bool noLoadPNG = false, bool noLoadStatus = true)
        {
            long pngSize = PngAssist.CheckSize(br);
            if (pngSize != 0L)
            {
                if (noLoadPNG)
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
                this.loadProductNo = br.ReadInt32();
                if (this.loadProductNo > 100)
                {
                    this.lastLoadErrorCode = -3;
                    return false;
                }
                
                if (br.ReadString() != "【AIS_Chara】")
                {
                    this.lastLoadErrorCode = -1;
                    return false;
                }
                
                this.loadVersion = new Version(br.ReadString());
                if (this.loadVersion > ChaFileDefine.ChaFileVersion)
                {
                    this.lastLoadErrorCode = -2;
                    return false;
                }
                
                this.language = br.ReadInt32();
                this.userID = br.ReadString();
                this.dataID = br.ReadString();
                
                int count = br.ReadInt32();
                BlockHeader blockHeader = MessagePackSerializer.Deserialize<BlockHeader>(br.ReadBytes(count));
                
                long num = br.ReadInt64();
                long position = br.BaseStream.Position;
                BlockHeader.Info infoCustom = blockHeader.SearchInfo(ChaFileCustom.BlockName);
                if (infoCustom != null)
                {
                    Version ver = new Version(infoCustom.version);
                    if (ver > ChaFileDefine.ChaFileCustomVersion)
                    {
                        this.lastLoadErrorCode = -2;
                    }
                    else
                    {
                        br.BaseStream.Seek(position + infoCustom.pos, SeekOrigin.Begin);
                        this.SetCustomBytes(br.ReadBytes((int)infoCustom.size), ver);
                    }
                }

                BlockHeader.Info infoCoord = blockHeader.SearchInfo(ChaFileCoordinate.BlockName);
                if (infoCoord != null)
                {
                    Version ver = new Version(infoCoord.version);
                    if (ver > ChaFileDefine.ChaFileCoordinateVersion)
                    {
                        this.lastLoadErrorCode = -2;
                    }
                    else
                    {
                        br.BaseStream.Seek(position + infoCoord.pos, SeekOrigin.Begin);
                        this.SetCoordinateBytes(br.ReadBytes((int)infoCoord.size), ver);
                    }
                }

                BlockHeader.Info infoParam = blockHeader.SearchInfo(ChaFileParameter.BlockName);
                if (infoParam != null)
                {
                    if (new Version(infoParam.version) > ChaFileDefine.ChaFileParameterVersion)
                    {
                        this.lastLoadErrorCode = -2;
                    }
                    else
                    {
                        br.BaseStream.Seek(position + infoParam.pos, SeekOrigin.Begin);
                        this.SetParameterBytes(br.ReadBytes((int)infoParam.size));
                    }
                }

                BlockHeader.Info infoGame = blockHeader.SearchInfo(ChaFileGameInfo.BlockName);
                if (infoGame != null)
                {
                    if (new Version(infoGame.version) > ChaFileDefine.ChaFileGameInfoVersion)
                    {
                        this.lastLoadErrorCode = -2;
                    }
                    else
                    {
                        br.BaseStream.Seek(position + infoGame.pos, SeekOrigin.Begin);
                        this.SetGameInfoBytes(br.ReadBytes((int)infoGame.size));
                    }
                }

                if (!noLoadStatus)
                {
                    BlockHeader.Info infoStatus = blockHeader.SearchInfo(ChaFileStatus.BlockName);
                    if (infoStatus != null)
                    {
                        if (new Version(infoStatus.version) > ChaFileDefine.ChaFileStatusVersion)
                        {
                            this.lastLoadErrorCode = -2;
                        }
                        else
                        {
                            br.BaseStream.Seek(position + infoStatus.pos, SeekOrigin.Begin);
                            this.SetStatusBytes(br.ReadBytes((int)infoStatus.size));
                        }
                    }
                }

                BlockHeader.Info infoParam2 = blockHeader.SearchInfo(ChaFileParameter2.BlockName);
                if (infoParam2 != null)
                {
                    if (new Version(infoParam2.version) > ChaFileDefine.ChaFileParameterVersion2)
                    {
                        this.lastLoadErrorCode = -2;
                    }
                    else
                    {
                        br.BaseStream.Seek(position + infoParam2.pos, SeekOrigin.Begin);
                        this.SetParameter2Bytes(br.ReadBytes((int)infoParam2.size));
                    }
                }

                BlockHeader.Info infoGame2 = blockHeader.SearchInfo(ChaFileGameInfo2.BlockName);
                if (infoGame2 != null)
                {
                    if (new Version(infoGame2.version) > ChaFileDefine.ChaFileGameInfoVersion2)
                    {
                        this.lastLoadErrorCode = -2;
                    }
                    else
                    {
                        br.BaseStream.Seek(position + infoGame2.pos, SeekOrigin.Begin);
                        this.SetGameInfo2Bytes(br.ReadBytes((int)infoGame2.size));
                    }
                }
                br.BaseStream.Seek(position + num, SeekOrigin.Begin);
            }
            catch (EndOfStreamException ex)
            {
                Debug.LogError(ex);
                this.lastLoadErrorCode = -999;
                return false;
            }
            this.lastLoadErrorCode = 0;

            return true;
        }

        public byte[] GetCustomBytes()
        {
            return GetCustomBytes(this.custom);
        }

        public static byte[] GetCustomBytes(ChaFileCustom _custom)
        {
            return _custom.SaveBytes();
        }

        public byte[] GetCoordinateBytes()
        {
            return GetCoordinateBytes(this.coordinate);
        }

        public static byte[] GetCoordinateBytes(ChaFileCoordinate _coordinate)
        {
            return _coordinate.SaveBytes();
        }

        public byte[] GetParameterBytes()
        {
            return GetParameterBytes(this.parameter);
        }

        public static byte[] GetParameterBytes(ChaFileParameter _parameter)
        {
            return MessagePackSerializer.Serialize(_parameter);
        }

        public byte[] GetParameter2Bytes()
        {
            return GetParameter2Bytes(this.parameter2);
        }

        public static byte[] GetParameter2Bytes(ChaFileParameter2 _parameter)
        {
            return MessagePackSerializer.Serialize(_parameter);
        }

        public byte[] GetGameInfoBytes()
        {
            return GetGameInfoBytes(this.gameinfo);
        }

        public static byte[] GetGameInfoBytes(ChaFileGameInfo _gameinfo)
        {
            return MessagePackSerializer.Serialize(_gameinfo);
        }

        public byte[] GetGameInfo2Bytes()
        {
            return GetGameInfo2Bytes(this.gameinfo2);
        }

        public static byte[] GetGameInfo2Bytes(ChaFileGameInfo2 _gameinfo)
        {
            return MessagePackSerializer.Serialize(_gameinfo);
        }

        public byte[] GetStatusBytes()
        {
            return GetStatusBytes(this.status);
        }

        public static byte[] GetStatusBytes(ChaFileStatus _status)
        {
            return MessagePackSerializer.Serialize(_status);
        }

        public void SetCustomBytes(byte[] data, Version ver)
        {
            this.custom.LoadBytes(data, ver);
        }

        public void SetCoordinateBytes(byte[] data, Version ver)
        {
            this.coordinate.LoadBytes(data, ver);
        }

        public void SetParameterBytes(byte[] data)
        {
            ChaFileParameter src = MessagePackSerializer.Deserialize<ChaFileParameter>(data);
            src.ComplementWithVersion();
            this.parameter.Copy(src);
        }

        public void SetParameter2Bytes(byte[] data)
        {
            ChaFileParameter2 src = MessagePackSerializer.Deserialize<ChaFileParameter2>(data);
            src.ComplementWithVersion();
            this.parameter2.Copy(src);
        }

        public void SetGameInfoBytes(byte[] data)
        {
            ChaFileGameInfo src = MessagePackSerializer.Deserialize<ChaFileGameInfo>(data);
            src.ComplementWithVersion();
            this.gameinfo.Copy(src);
        }

        public void SetGameInfo2Bytes(byte[] data)
        {
            ChaFileGameInfo2 src = MessagePackSerializer.Deserialize<ChaFileGameInfo2>(data);
            src.ComplementWithVersion();
            this.gameinfo2.Copy(src);
        }

        public void SetStatusBytes(byte[] data)
        {
            ChaFileStatus src = MessagePackSerializer.Deserialize<ChaFileStatus>(data);
            src.ComplementWithVersion();
            this.status.Copy(src);
        }

        public void CopyAll(
          ChaFile _chafile,
          bool _custom = true,
          bool _coordinate = true,
          bool _parameter = true,
          bool _gameinfo = true,
          bool _status = true)
        {
            if (_custom)
                this.CopyCustom(_chafile.custom);
            if (_coordinate)
                this.CopyCoordinate(_chafile.coordinate);
            if (_status)
                this.CopyStatus(_chafile.status);
            if (_parameter)
            {
                this.CopyParameter(_chafile.parameter);
                this.CopyParameter2(_chafile.parameter2);
            }
            if (_gameinfo)
            {
                this.CopyGameInfo(_chafile.gameinfo);
                this.CopyGameInfo2(_chafile.gameinfo2);
            }
        }

        public void CopyCustom(ChaFileCustom _custom)
        {
            this.SetCustomBytes(GetCustomBytes(_custom), ChaFileDefine.ChaFileCustomVersion);
        }

        public void CopyCoordinate(ChaFileCoordinate _coordinate)
        {
            this.SetCoordinateBytes(GetCoordinateBytes(_coordinate), ChaFileDefine.ChaFileCoordinateVersion);
        }

        public void CopyParameter(ChaFileParameter _parameter)
        {
            this.SetParameterBytes(GetParameterBytes(_parameter));
        }

        public void CopyParameter2(ChaFileParameter2 _parameter)
        {
            this.SetParameter2Bytes(GetParameter2Bytes(_parameter));
        }

        public void CopyGameInfo(ChaFileGameInfo _gameinfo)
        {
            this.SetGameInfoBytes(GetGameInfoBytes(_gameinfo));
        }

        public void CopyGameInfo2(ChaFileGameInfo2 _gameinfo)
        {
            this.SetGameInfo2Bytes(GetGameInfo2Bytes(_gameinfo));
        }

        public void CopyStatus(ChaFileStatus _status)
        {
            this.SetStatusBytes(GetStatusBytes(_status));
        }
    }
}
