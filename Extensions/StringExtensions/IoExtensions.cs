using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extensions.StringExtensions
{
    public static class IoExtensions
    {

        /// <summary>
        /// Will make sure all chars that are illegal in file names are removed from the given filename.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string MakeSafeForFilename(this string str, string replaceWith = "")
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                str = str.Replace(c.ToString(), replaceWith);
            return str;
        }

        /// <summary>
        /// Will replace the file extension of the given filename with the new extension.
        /// </summary>
        /// <param name="oldFilename">Full path</param>
        /// <param name="newExtension">The new extension (for example ".ldb")</param>
        /// <returns></returns>
        public static string ChangeExtension(this string oldFilename, string newExtension)
        {
            if (string.IsNullOrEmpty(oldFilename)) return oldFilename;
            var dotIx = oldFilename.LastIndexOf(".", StringComparison.InvariantCulture);
            if (dotIx < 0) return oldFilename + newExtension;
            var dir = Path.GetDirectoryName(oldFilename);
            if (dir != null && dotIx < dir.Length) return oldFilename + newExtension;
            return oldFilename.Substring(0, dotIx) + newExtension;
        }

        /// <summary>
        /// Adds the given suffix to the filename directly before the extension.
        /// </summary>
        /// <param name="filename">The original path/filename</param>
        /// <param name="suffix">The suffix to add before the extension</param>
        /// <returns></returns>
        public static string AddSuffixBeforeExtension(this string filename, string suffix)
        {
            var extension = Path.GetExtension(filename);
            if (string.IsNullOrWhiteSpace(extension)) return filename + suffix;
            return filename.Substring(0, filename.Length - extension.Length) + suffix + extension;
        }

        /// <summary>
        /// Will remove the file extension of the given filename.
        /// </summary>
        /// <param name="oldFilename">Full path</param>
        /// <returns></returns>
        public static string RemoveExtension(this string oldFilename)
        {
            if (string.IsNullOrWhiteSpace(oldFilename)) return oldFilename;
            return Path.Combine(Path.GetDirectoryName(oldFilename), Path.GetFileNameWithoutExtension(oldFilename));
        }

        /// <summary>
        /// Will truncate a file path by replacing entire folder names with an ellipsis. For example: C:\folder\one\two\three\file.txt => C:\folder\...\three\file.txt
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string TruncatePath(this string path, int length)
        {
            Func<string[], string> assemble = parts =>
            {
                var compressed = new List<string>();
                foreach (var part in parts)
                    if (part != null || compressed.Last() != null)
                        compressed.Add(part);

                return string.Join(Path.DirectorySeparatorChar.ToString(), compressed.Select(p => p ?? "...").ToArray());
            };

            var pathParts = path.Split(Path.DirectorySeparatorChar);
            while (path.Length > length)
            {
                if (pathParts.Count(p => p != null) <= 2) break;

                var middle = path.Length / 2;
                var lengthSoFar = 0;
                var middleIndex = 0;
                foreach (var p in pathParts)
                {
                    if (p != null)
                        lengthSoFar += p.Length + 1;
                    if (lengthSoFar >= middle)
                        break;
                    middleIndex++;
                }

                while (middleIndex >= 0 && (middleIndex == pathParts.Length - 1 || pathParts[middleIndex] == null)) middleIndex--;
                if (middleIndex < 1) break;

                pathParts[middleIndex] = null;

                path = assemble(pathParts);

            }

            return path;
        }
    }
}