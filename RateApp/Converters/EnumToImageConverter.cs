using RateApp.Model;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace RateApp.Converters
{
    public class EnumToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((RateIndicator)value)
            {
                case RateIndicator.Decreased:
                    return new BitmapImage(new Uri("ms-appx://RateApp/Resources/arrow-down.png"));

                case RateIndicator.Equal:
                    return new BitmapImage(new Uri("ms-appx://RateApp/Resources/equal.png"));

                case RateIndicator.Increased:
                    return new BitmapImage(new Uri("ms-appx://RateApp/Resources/arrow-up.png"));

                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
