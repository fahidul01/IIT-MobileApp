using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.AppServices
{
    public interface INotificationService
    {
        Task SendNotification(string topic, string title, string message);
    }
}
