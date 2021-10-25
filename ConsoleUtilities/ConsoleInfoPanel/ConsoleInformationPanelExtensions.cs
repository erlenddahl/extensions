using System.Collections.Generic;

namespace ConsoleUtilities.ConsoleInfoPanel
{
    public static class ConsoleInformationPanelExtensions
    {
        /// <summary>
        /// Enumerates the given items. If the cip object is null, this acts as a normal enumerator. Otherwise, it creates
        /// and updates a progressbar with the given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cip"></param>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> Run<T>(this ConsoleInformationPanel cip, string key, ICollection<T> items, int? sequence = null)
        {
            if (cip == null)
            {
                foreach (var item in items)
                    yield return item;
                yield break;
            }

            using (var pb = cip.SetProgress(key, max: items.Count, sequence: sequence))
            {
                foreach (var item in items)
                {
                    yield return item;
                    pb.Increment();
                }
            }
        }
    }
}