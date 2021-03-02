using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Utilities
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
