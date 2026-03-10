using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Utils
{
    internal class UriUtils
    {
        //yeah this code is really ineffecient but it won't be called super often.
        //To speed up: create a version that takes a base path and a list of things to append
        //Then use stringbuilder instead of + or append.
        /// <summary>
        /// Takes two paths and appends them with a '/' separating character.
        /// </summary>
        /// <param name="path1">The base path to be combined</param>
        /// <param name="path2">The path to append to the end of the base path</param>
        /// <returns>The combined path, this path does not feature a trailing '/'</returns>
        public static string AppendPath(string path1, string path2)
        {
            string basePath = path1.TrimEnd('/') + "/"; //ensure the path ends with /
            string combinedPath = basePath + path2.Trim('/'); //add the new path, but ensure path2 doesn't lead with / (or end with /)
            return combinedPath;
        }
    }
}
