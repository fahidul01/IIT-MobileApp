using Android.App;
using Android.Widget;
using Mobile.Core.Engines.Services;

namespace MobileApp.Droid.Services
{
    internal class PlatformService : IPlatformService
    {
        public void OpenToast(string text)
        {
            Toast.MakeText(Application.Context, text, ToastLength.Short).Show();
        }
    }
}