using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CharaTools
{
    public static class DeepCopyHelper
    {
        public static T DeepCopy<T>(T target)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            try
            {
#pragma warning disable SYSLIB0011
                binaryFormatter.Serialize(memoryStream, target);
                memoryStream.Position = 0L;
                return (T)binaryFormatter.Deserialize(memoryStream);
#pragma warning restore SYSLIB0011
            }
            finally
            {
                memoryStream.Close();
            }
        }
    }
}
