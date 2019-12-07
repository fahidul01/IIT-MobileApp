
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage(Page page)
        {
            InitializeComponent();
            Detail = page;
        }
    }
}