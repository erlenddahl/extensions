namespace net.erlenddahl.Extensions.Utilities
{
    public class Wrapper<T>
    {
        public T Value { get; set; }

        public Wrapper(T initialValue)
        {
            Value = initialValue;
        }
    }
}
