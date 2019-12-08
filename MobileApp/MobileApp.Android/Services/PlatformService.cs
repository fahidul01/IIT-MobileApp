using Android.App;
using Android.Widget;
using Firebase.Messaging;
using Mobile.Core.Engines.Services;

namespace MobileApp.Droid.Services
{
    internal class PlatformService : IPlatformService
    {
        public void OpenToast(string text)
        {
            Toast.MakeText(Application.Context, text, ToastLength.Short).Show();
        }
        public void SubsubcribeTopics(params string[] topics)
        {
            foreach (var item in topics)
            {
                FirebaseMessaging.Instance.SubscribeToTopic(item);
            }
        }

        public void UnsubscribeTopics(params string[] topics)
        {
            foreach (var item in topics)
            {
                FirebaseMessaging.Instance.UnsubscribeFromTopic(item);
            }
        }
    }
}