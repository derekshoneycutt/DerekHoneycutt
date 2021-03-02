using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services.Core
{
    /// <summary>
    /// Email service for sending emails to users
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// SMTP settings for the application
        /// </summary>
        private readonly Config.SmtpSettings SmtpSettings;

        public EmailService(IOptions<Config.SmtpSettings> smtpSettings)
        {
            SmtpSettings = smtpSettings.Value;
        }

        /// <summary>
        /// Send an email from the application
        /// </summary>
        /// <param name="to">Address to send the email to</param>
        /// <param name="subject">Subject of the email to send</param>
        /// <param name="html">HTML Body of the email to send</param>
        /// <param name="from">Address of the sender for the email</param>
        public async Task Send(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? SmtpSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(SmtpSettings.Server, SmtpSettings.Port);
            await smtp.AuthenticateAsync(SmtpSettings.Username, SmtpSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
