﻿using Microsoft.Extensions.Options;
using Student.Infrastructure.AppServices;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IIT.Server.WebServices
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

            try
            {
#if DEBUG
                Console.WriteLine("Email Demo: " + email);
                await Task.Delay(100);
#else

                using (var client = new SmtpClient())
                {
                    var mail = new MailMessage(Options.From, email)
                    {
                        Subject = subject,
                        Body = message
                    };


                    client.Host = Options.SMTPServer;
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Options.Username, Options.Password);

                    await client.SendMailAsync(mail);
                }
#endif
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
