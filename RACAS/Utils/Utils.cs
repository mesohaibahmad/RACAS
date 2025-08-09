using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;


    public static class Utils
    {
        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }
                }
            }

            return val;
        }

        public static bool In(this string source, params string[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source, StringComparer.OrdinalIgnoreCase);
        }

    //public static T DeepClone<T>(this T a)
    //{
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        formatter.Serialize(stream, a);
    //        stream.Position = 0;
    //        return (T)formatter.Deserialize(stream);
    //    }
    //}
    public static T DeepClone<T>(this T source)
    {
        if (source == null)
            return default!;

        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true, // Optional: include public fields if needed
        };

        var json = JsonSerializer.Serialize(source, options);
        return JsonSerializer.Deserialize<T>(json, options)!;
    }

}

