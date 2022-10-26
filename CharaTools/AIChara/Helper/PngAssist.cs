using System;
using System.IO;

namespace CharaTools.AIChara
{
    public static class PngAssist
    {
        public static void SkipPng(Stream st)
        {
            long size = 0L;
            CheckPngData(st, ref size, true);
        }

        public static void SkipPng(BinaryReader br)
        {
            long size = 0L;
            CheckPngData(br, ref size, true);
        }

        public static long CheckSize(BinaryReader reader)
        {
            long size = 0L;
            if (CheckPngData(reader, ref size, false))
                return size;
            return 0L;
        }

        public static long CheckSize(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(memoryStream))
                    return CheckSize(reader);
            }
        }

        public static bool CheckPngData(Stream st, ref long size, bool skip)
        {
            if (st == null)
                return false;
            size = 0L;
            long position = st.Position;
            byte[] buffer1 = new byte[8];
            byte[] numArray = new byte[8]
            {
                137, 80, 78, 71, 13, 10, 26, 10
            };
            st.Read(buffer1, 0, 8);
            for (int index = 0; index < 8; ++index)
            {
                if (buffer1[index] != numArray[index])
                {
                    st.Seek(position, SeekOrigin.Begin);
                    return false;
                }
            }
            bool flag = true;
            while (flag)
            {
                byte[] buffer2 = new byte[4];
                st.Read(buffer2, 0, 4);
                Array.Reverse(buffer2);
                int int32 = BitConverter.ToInt32(buffer2, 0);
                byte[] buffer3 = new byte[4];
                st.Read(buffer3, 0, 4);
                if (BitConverter.ToInt32(buffer3, 0) == 1145980233)
                    flag = false;
                st.Seek(int32 + 4, SeekOrigin.Current);
            }
            size = st.Position - position;
            if (!skip)
                st.Seek(position, SeekOrigin.Begin);
            return true;
        }

        public static bool CheckPngData(BinaryReader reader, ref long size, bool skip)
        {
            return reader != null && CheckPngData(reader.BaseStream, ref size, skip);
        }

        public static bool CheckPngData(byte[] data, ref long size)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                return CheckPngData(memoryStream, ref size, false);
            }
        }

        public static byte[] LoadPngData(string fullpath)
        {
            using (FileStream fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                    return LoadPngData(reader);
            }
        }

        public static byte[] LoadPngData(BinaryReader reader)
        {
            if (reader == null)
                return null;
            long size = 0;
            CheckPngData(reader.BaseStream, ref size, false);
            return size == 0L ? null : reader.ReadBytes((int)size);
        }
    }
}
