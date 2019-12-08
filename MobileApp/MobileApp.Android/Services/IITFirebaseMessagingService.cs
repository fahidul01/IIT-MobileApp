﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Mobile.Core.Models.Core;

namespace MobileApp.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class IITFirebaseMessagingService : FirebaseMessagingService
    {
        //private static int NotificationID = 0;
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            var body = message.GetNotification();
            SendNotification(body, message.Data);
        }

        void SendNotification(RemoteMessage.Notification message, IDictionary<string, string> data)
        {
            using (var intent = new Intent(this, typeof(MainActivity)))
            {
                intent.AddFlags(ActivityFlags.ClearTop);
                if (data != null)
                {
                    foreach (var key in data.Keys)
                    {
                        intent.PutExtra(key, data[key]);
                    }
                }
            }

            AppService.ShowAlert(message.Body, message.Title);
        }
    }
}