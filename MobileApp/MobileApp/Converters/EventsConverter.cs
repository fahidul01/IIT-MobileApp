using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace MobileApp.Converters
{
    public class EventsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventCol = new EventCollection();
            if (value is List<Notice> notices && notices.Count > 0)
            {
                var grouped = notices.GroupBy(x => x.EventDate);
               
                foreach(var group in grouped)
                {
                    var data = group.ToList();
                    eventCol.Add(group.Key, data);
                }
            }
            return eventCol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
