using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Utils
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
