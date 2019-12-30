using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalenderView : ContentView
    {
        public DayItem SelectedItem { get; set; }
        public List<DayItem> DayItems { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        public CalenderView()
        {
            InitializeComponent();
            DayItems = new List<DayItem>();
            for (var counter = 0; counter < 35; counter++)
                DayItems.Add(new DayItem());
        }

        public ICommand ItemSelectedCommand { get; set; }
        public ActivityCollection ActivityCollection { get; set; }

        public static BindableProperty ActivityCollectionProperty =
            BindableProperty.Create(
                nameof(ActivityCollection),
                typeof(ActivityCollection),
                typeof(CalenderView),
                propertyChanged: ActivityCollectionChanged);
        private static void ActivityCollectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is CalenderView && newValue is ActivityCollection collection)
            {

            }
        }
    }

    public class DayItem
    {
        public DateTime DateTime { get; set; }
        public string Day => DateTime.Day.ToString();
        public Color BackgroundColor { get; set; } = Color.Transparent;
        public Color ForegroundColor { get; set; } = Color.Black;
        public Color HasItemColor { get; set; } = Color.Maroon;
    }
}