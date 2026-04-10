namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel
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
            return cip.Run(key, items, items.Count, sequence);
        }

        /// <summary>
        /// Enumerates the given items. If the cip object is null, this acts as a normal enumerator. Otherwise, it creates
        /// and updates a progressbar with the given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cip"></param>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> Run<T>(this ConsoleInformationPanel cip, string key, IEnumerable<T> items, int count, int? sequence = null)
        {
            if (cip == null)
            {
                foreach (var item in items)
                    yield return item;
                yield break;
            }

            using (var pb = cip.SetProgress(key, max: count, sequence: sequence))
            {
                foreach (var item in items)
                {
                    yield return item;
                    pb.Increment();
                }
            }
        }

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
        public static IEnumerable<T> Run<T>(this ConsoleInformationPanel cip, string key, IEnumerable<T> items, int? sequence = null)
        {
            if (cip == null)
            {
                foreach (var item in items)
                    yield return item;
                yield break;
            }

            using (var pb = cip.SetUnknownProgress(key, sequence: sequence))
            {
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}