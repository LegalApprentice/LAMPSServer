using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace LAMPSServer.Helpers
{

    public class ByteData
    {
        public byte[] Data { get; set; }
    }

    public static class SmartHelpers
    {
        //private static void UpdateForType<T,N>(T source, N destination) where T: CaseCoreInfo, where N: CaseCoreInfo
        //{
        //    FieldInfo[] myObjectFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        //    foreach (FieldInfo fi in myObjectFields)
        //    {
        //        fi.SetValue(destination, fi.GetValue(source));
        //    }
        //}

        // public static T DeepCopy<T>(T obj)
        // {
        //     if (!typeof(T).IsSerializable)
        //     {
        //         throw new Exception("The source object must be serializable");
        //     }

        //     if (Object.ReferenceEquals(obj, null))
        //     {
        //         throw new Exception("The source object must not be null");
        //     }

        //     T result = default(T);

        //     using (var memoryStream = new MemoryStream())
        //     {
        //         var formatter = new BinaryFormatter();
        //         formatter.Serialize(memoryStream, obj);
        //         memoryStream.Seek(0, SeekOrigin.Begin);
        //         result = (T)formatter.Deserialize(memoryStream);
        //         memoryStream.Close();
        //     }
        //     return result;
        // }
    }
}
