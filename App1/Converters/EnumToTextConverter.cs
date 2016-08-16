using RateApp.Model;
using System;
using Windows.UI.Xaml.Data;

namespace RateApp.Converters
{
    public class EnumToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((RateIndicator)value)
            {
                case RateIndicator.Decreased:
                    return "Este valor ha decendido en relación a la fecha precedente";

                case RateIndicator.Equal:
                    return "Este valor se ha mantenido igual en relación a la fecha precedente";

                case RateIndicator.Increased:
                    return "Este valor ha aumentado en relación a la fecha precedente";

                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
