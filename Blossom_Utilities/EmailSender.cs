using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Blossom_Utilities.Configurations;
using Microsoft.Extensions.Configuration;

namespace Blossom_Utilities
{
    public class EmailSender
    {
        private readonly EmailConfiguration _smtpSettings;

        public EmailSender(IConfiguration configuration)
        {
            _smtpSettings = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using (var client = new SmtpClient())
            {
                client.Host = _smtpSettings.Host;
                client.Port = _smtpSettings.Port;
                client.EnableSsl = _smtpSettings.EnableSSL;
                client.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromEmail),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true // Enable HTML content
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
