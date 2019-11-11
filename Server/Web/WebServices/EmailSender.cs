using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Web.Infrastructure.AppServices;

namespace Web.WebServices
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }


        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(email, subject, message);
        }

        public Task Execute(string email, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                Body = message,
                Subject = subject,
                From = new MailAddress(Options.From)
            };
            mailMessage.To.Add(email);


            using var client = new SmtpClient(Options.SMTPServer)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Options.Username, Options.Password)
            };
            return client.SendMailAsync(mailMessage);

        }
    }
}
