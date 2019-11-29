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


        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            return await Execute(email, subject, message);
        }

        private async Task<bool> Execute(string email, string subject, string message)
        {
            var mailMessage = new MailMessage()
            {
                Body = message,
                Subject = subject,
                From = new MailAddress(Options.From,"IIT WebMail"),
            };
            mailMessage.To.Add(email);


            using var client = new SmtpClient(Options.SMTPServer)
            {
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true
            };
            client.Credentials = new NetworkCredential(Options.From, Options.Password);
            await client.SendMailAsync(mailMessage);
            return true;
        }
    }
}
