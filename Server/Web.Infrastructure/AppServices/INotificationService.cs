using System.Threading.Tasks;

namespace Web.Infrastructure.AppServices
{
    public interface INotificationService
    {
        void SendNotification(string topic, string title, string message);
    }
}
