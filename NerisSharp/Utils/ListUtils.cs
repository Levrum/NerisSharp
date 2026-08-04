using System.Collections.Generic;

namespace NerisSharp.Utils
{
    internal static class ListUtils
    {
        internal static void AddIfNotNull(this List<string> list, string ToAdd)
        {
            if (ToAdd != null)
            {
                list.Add(ToAdd);
            }
        }
    }
}
