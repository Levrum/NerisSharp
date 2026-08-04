using System;
using System.Collections.Generic;
using System.Linq;

namespace NerisSharp.Utils
{
    internal static class EnumUtils
    {
        public static List<string> ReturnStringList<T>(List<T> enumType)
        {
            if (enumType == null) return null;
            Type tType = typeof(T);
            if (!tType.IsEnum)
            {
                throw new ArgumentException("Only enum types allowed");
            }
            List<string> toReturn = enumType.Select(x => x.ToString()).ToList();
            return toReturn;
        }
    }
}
