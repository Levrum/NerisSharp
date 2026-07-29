using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NerisRunner
{
    internal class Utils
    {
        public static string SerializeJsonPretty<T>(T obj)
        {
            JsonSerializerOptions PrettyPrintLowerCase = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            string retVal = JsonSerializer.Serialize(obj, PrettyPrintLowerCase);
            return retVal;
        }

        public static string SerializeJsonPrettyWithNulls<T>(T obj)
        {
            JsonSerializerOptions PrettyPrintLowerCase = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                WriteIndented = true,
                Converters = {new JsonStringEnumConverter()}
            };
            string retVal = JsonSerializer.Serialize(obj, PrettyPrintLowerCase);
            return retVal;
        }

        public static int GetEnumValueCount<T>()
        {
            Type t = typeof(T);
            if (!t.IsEnum) throw new ArgumentException("Must be enum type");
            return Enum.GetNames(t).Length;
        }
    }
}
