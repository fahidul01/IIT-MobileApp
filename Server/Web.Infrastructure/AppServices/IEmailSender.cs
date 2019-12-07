using System.Threading.Tasks;

namespace Web.Infrastructure.AppServices
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }

    public class AuthMessageSenderOptions
    {
        public string SMTPServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }
}
