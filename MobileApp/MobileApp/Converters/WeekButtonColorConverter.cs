using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    public class WeekButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool data && data)
            {
                return Color.WhiteSmoke;
            }
            else
            {
                return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
