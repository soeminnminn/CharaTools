using System;
using System.IO;
using MessagePack;
using Newtonsoft.Json;

namespace CharaTools.AIChara
{
    public class ChaFileCustom : ChaFileAssist
    {
        [JsonIgnore]
        public static readonly string BlockName = "Custom";

        public ChaFileFace face;
        public ChaFileBody body;
        public ChaFileHair hair;

        public ChaFileCustom()
        {
            this.MemberInit();
        }

        public void MemberInit()
        {
            this.face = new ChaFileFace();
            this.body = new ChaFileBody();
            this.hair = new ChaFileHair();
        }

        public byte[] SaveBytes()
        {
            byte[] bufferFace = MessagePackSerializer.Serialize(this.face);
            byte[] bufferBody = MessagePackSerializer.Serialize(this.body);
            byte[] bufferHair = MessagePackSerializer.Serialize(this.hair);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(bufferFace.Length);
                    binaryWriter.Write(bufferFace);
                    binaryWriter.Write(bufferBody.Length);
                    binaryWriter.Write(bufferBody);
                    binaryWriter.Write(bufferHair.Length);
                    binaryWriter.Write(bufferHair);
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
                    int countFace = binaryReader.ReadInt32();
                    this.face = MessagePackSerializer.Deserialize<ChaFileFace>(binaryReader.ReadBytes(countFace));
                    
                    int countBody = binaryReader.ReadInt32();
                    this.body = MessagePackSerializer.Deserialize<ChaFileBody>(binaryReader.ReadBytes(countBody));
                    
                    int countHair = binaryReader.ReadInt32();
                    this.hair = MessagePackSerializer.Deserialize<ChaFileHair>(binaryReader.ReadBytes(countHair));
                    
                    this.face.ComplementWithVersion();
                    this.body.ComplementWithVersion();
                    this.hair.ComplementWithVersion();

                    return true;
                }
            }
        }

        public void SaveFace(string path)
        {
            this.SaveFileAssist(path, this.face);
        }

        public void LoadFace(string path)
        {
            this.LoadFileAssist(path, out this.face);
            this.face.ComplementWithVersion();
        }

        public void LoadFace(byte[] bytes)
        {
            this.LoadFileAssist(bytes, out this.face);
            this.face.ComplementWithVersion();
        }

        public void SaveBody(string path)
        {
            this.SaveFileAssist(path, this.body);
        }

        public void LoadBody(string path)
        {
            this.LoadFileAssist(path, out this.body);
            this.body.ComplementWithVersion();
        }

        public void SaveHair(string path)
        {
            this.SaveFileAssist(path, this.hair);
        }

        public void LoadHair(string path)
        {
            this.LoadFileAssist(path, out this.hair);
            this.hair.ComplementWithVersion();
        }

        public int GetBustSizeKind()
        {
            int kind = 1;
            float shape = this.body.shapeValueBody[1];
            if (0.330000013113022 >= shape)
                kind = 0;
            else if (0.660000026226044 <= shape)
                kind = 2;
            return kind;
        }

        public int GetHeightKind()
        {
            int kind = 1;
            float shape = this.body.shapeValueBody[0];
            if (0.330000013113022 >= shape)
                kind = 0;
            else if (0.660000026226044 <= shape)
                kind = 2;
            return kind;
        }
    }
}
