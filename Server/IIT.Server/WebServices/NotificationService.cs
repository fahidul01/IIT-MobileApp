using CoreEngine.Engine;
using Newtonsoft.Json.Linq;
using Student.Infrastructure.AppServices;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IIT.Server.WebServices
{
    public class NotificationService : INotificationService
    {
        private const string GCMUrl = "https://fcm.googleapis.com/fcm/send";
        private const string GCMKey = "AAAAjxgurSQ:APA91bHizujj6fQc4IxbMY81tRuLNKDfca_wiqNDM4hyX7ZfyDCqe7-Tn4hd8EiaOFRhEbIY2Ktr5NLV5f5uLtuHjTk3X6Taw4_1Zqtj91tECt4XkurcPZPBVqcFd_M_2sbhb5Sn0NhO";
        public async void SendNotification(string topic, string title, string message)
        {
            try
            {
                var notification = new JObject
                {
                    ["body"] = message,
                    ["title"] = title,
                    ["sound"] = "default"
                };
                var topicData = new JObject
                {
                    ["to"] = "/topics/" + topic.ToLower().Replace(" ", ""),
                    ["priority"] = "high",
                    ["notification"] = notification,
                    ["ttl"] = 3600
                };
                var sData = topicData.ToString();
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GCMKey);

#if DEBUG
                await Task.Delay(100);
                Console.WriteLine(string.Format("   Notification demo: {0}=> {1}, {2}",
                                topic, title, message));
#else
                var resp = await httpClient.PostAsync(GCMUrl, new StringContent(sData));
                Console.WriteLine("Notification:" + resp.StatusCode);
#endif

            }
            catch (Exception ex)
            {
                LogEngine.Error(ex);
            }
        }
    }
}
