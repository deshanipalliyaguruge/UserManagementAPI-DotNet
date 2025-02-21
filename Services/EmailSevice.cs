using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UserManagementAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            Console.WriteLine("Sending email to: " + toEmail);
            Console.WriteLine("Subject: " + subject);
            Console.WriteLine("Body: " + body);
            var smtpServer = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SmtpUser"];
            var senderPassword = _configuration["EmailSettings:SmtpPass"];
            Console.WriteLine("Sender email: " + senderEmail);
            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;  //  Ensure SSL is enabled
                client.UseDefaultCredentials = false;  //  Disable default credentials
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(new MailAddress(toEmail));

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
