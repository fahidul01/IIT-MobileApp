using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Notice
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoticesPage : CustomPage<NoticesViewModel>
    {
        public NoticesPage()
        {
            InitializeComponent();
        }
    }
}