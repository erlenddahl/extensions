using System.IO;
using System.Reflection;

namespace net.erlenddahl.Extensions.AssemblyExtensions
{
    public static class Resources
    {
        /// <summary>
        /// Returns the string contents of the given resource. The resource name is usually something like
        /// Full.Assemby.Namespace.Resources.Filename.Extension. Use GetEmbeddedResourceNames() to extract a list of names.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string ReadResourceFile(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Get the list of all emdedded resources in the assembly.
        /// </summary>
        /// <returns>An array of fully qualified resource names</returns>
        public static string[] GetEmbeddedResourceNames()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }
    }
}