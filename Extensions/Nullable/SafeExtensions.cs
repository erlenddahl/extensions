namespace net.erlenddahl.Extensions.Nullable
{
    public static class SafeExtensions
    {
        public static T Safe<T>(this T? item, T defaultValue = default(T)) where T : struct
        {
            if (item.HasValue) return item.Value;
            return defaultValue;
        }
    }
}