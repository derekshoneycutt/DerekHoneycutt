using DerekHoneycutt.Data.Services.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.Services.Implementation
{
    /// <summary>
    /// Email service for sending emails to users
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// SMTP settings for the application
        /// </summary>
        private readonly IOptions<Options.SmtpSettings> SmtpSettings;

        public EmailService(IOptions<Options.SmtpSettings> smtpSettings)
        {
            SmtpSettings = smtpSettings;
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
            var settings = SmtpSettings.Value;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? settings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(settings.Server, settings.Port);
            await smtp.AuthenticateAsync(settings.Username, settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
