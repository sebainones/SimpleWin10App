using System;

namespace RateApp.Converters
{
    public static class Converter
    {
        public static T ConvertValue<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
