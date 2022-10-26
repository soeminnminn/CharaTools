using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CharaTools
{
    public static class Extensions
    {
        public static object DeepCopy(this object target)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            try
            {
#pragma warning disable SYSLIB0011
                binaryFormatter.Serialize(memoryStream, target);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return binaryFormatter.Deserialize(memoryStream);
#pragma warning restore SYSLIB0011
            }
            finally
            {
                memoryStream.Close();
            }
        }
    }
}
