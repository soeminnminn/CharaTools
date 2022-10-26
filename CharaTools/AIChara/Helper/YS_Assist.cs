using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace CharaTools.AIChara
{
    public static class YS_Assist
    {
        private static readonly string passwordChars36 = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly string passwordChars62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static readonly char[] tbl62 = new char[62]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        private static readonly byte[] tblRevCode = new byte[128]
        {
            50, 112, 114, 160, 140, 152, 202, 10, 235, 9, 198, 113, 78, 208, 182, 154, 247, 249, 64, 243, 232, 102, 184, 130,
            196, 33, 149, 171, 62, 235, 124, 183, 193, 189, 168, 165, 243, 117, 48, 23, 16, 114, 192, 105, 122, 253, 206, 143,
            240, 183, 150, 127, 115, 117, 19, 135, 140, 187, 73, 133, 254, 231, 48, 92, 205, 127, 122, 237, 250, 212, 27, 92,
            153, 237, 54, 161, 135, 216, 104, 10, 60, 128, 97, 33, 47, 124, 18, 218, 168, 133, 217, 249, 188, 179, 198, 104,
            68, 229, 179, 61, 10, 22, 10, 183, 8, 189, 74, 86, 107, 47, 230, 233, 158, 241, 27, 85, 198, 164, 151, 135, 148,
            4, 24, 172, 122, 214, 18, 140
        };

        public static void BitRevBytes(byte[] data, int startPos)
        {
            int index1 = startPos % tblRevCode.Length;
            for (int index2 = 0; index2 < data.Length; ++index2)
            {
                int index3 = index2;
                data[index3] ^= tblRevCode[index1];
                index1 = (index1 + 1) % tblRevCode.Length;
            }
        }

        public static string Convert64StringFromInt32(int num)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            if (num < 0)
            {
                num *= -1;
                stringBuilder1.Append("0");
            }
            for (; num >= 62; num /= 62)
                stringBuilder1.Append(tbl62[num % 62]);
            stringBuilder1.Append(tbl62[num]);
            StringBuilder stringBuilder2 = new StringBuilder();
            for (int index = stringBuilder1.Length - 1; index >= 0; --index)
                stringBuilder2.Append(stringBuilder1[index]);
            return stringBuilder2.ToString();
        }

        public static string GeneratePassword36(int length)
        {
            StringBuilder stringBuilder = new StringBuilder(length);
            // byte[] data = new byte[length];
            // new RNGCryptoServiceProvider().GetBytes(data);
            byte[] data = RandomNumberGenerator.GetBytes(length);
            for (int index1 = 0; index1 < length; ++index1)
            {
                int index2 = data[index1] % passwordChars36.Length;
                char ch = passwordChars36[index2];
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }

        public static string GeneratePassword62(int length)
        {
            StringBuilder stringBuilder = new StringBuilder(length);
            // byte[] data = new byte[length];
            // new RNGCryptoServiceProvider().GetBytes(data);
            byte[] data = RandomNumberGenerator.GetBytes(length);
            for (int index1 = 0; index1 < length; ++index1)
            {
                int index2 = data[index1] % passwordChars62.Length;
                char ch = passwordChars62[index2];
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }

        public static string GetMacAddress()
        {
            string empty = string.Empty;
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (networkInterfaces != null)
            {
                foreach (NetworkInterface networkInterface in networkInterfaces)
                {
                    PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
                    byte[] numArray = null;
                    if (physicalAddress != null)
                        numArray = physicalAddress.GetAddressBytes();
                    if (numArray != null && numArray.Length == 6)
                    {
                        empty += physicalAddress.ToString();
                        break;
                    }
                }
            }
            return empty;
        }

        public static string CreateUUID()
        {
            StringBuilder stringBuilder = new StringBuilder(32);
            stringBuilder.Append(GetMacAddress());

            if (string.Empty == stringBuilder.ToString())
                stringBuilder.Append(GeneratePassword36(12));

            stringBuilder.Append(DateTime.Now.ToString("MMddHHmmssfff"));
            stringBuilder.Append(GeneratePassword36(40));

            byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            BitRevBytes(bytes, 7);

            stringBuilder.Length = 0;

            int num1 = bytes.Length / 4;
            int num2 = bytes.Length % 4;
            if (num2 != 0)
            {
                int length1 = 4 - num2;
                byte[] numArray1 = new byte[length1];
                int length2 = bytes.Length;
                Array.Resize<byte>(ref bytes, length2 + length1);
                byte[] numArray2 = bytes;
                int destinationIndex = length2;
                int length3 = length1;
                Array.Copy(numArray1, 0, numArray2, destinationIndex, length3);
                ++num1;
            }

            for (int index = 0; index < num1; ++index)
                stringBuilder.Append(Convert64StringFromInt32(BitConverter.ToInt32(bytes, index * 4)));

            string str;
            if (stringBuilder.Length < 64)
            {
                int length = 64 - stringBuilder.Length;
                stringBuilder.Append(GeneratePassword62(length));
                str = stringBuilder.ToString();
            }
            else
                str = stringBuilder.ToString().Substring(0, 64);

            return str;
        }
    }
}
