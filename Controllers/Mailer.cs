using DerekHoneycutt.RestModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Defines the standard mailer using MailKit
    /// </summary>
    public class Mailer : IMailer
    {
        /// <summary>
        /// SmtpSettings from the application
        /// </summary>
        private readonly BusinessModels.SmtpSettings _smtpSettings;
        /// <summary>
        /// WebHostEnvironment data to use if needed
        /// </summary>
        private readonly IWebHostEnvironment _env;

        public Mailer(IOptions<BusinessModels.SmtpSettings> options, IWebHostEnvironment env)
        {
            _smtpSettings = options.Value;
            _env = env;
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="sendto">Email address to send to</param>
        /// <param name="data">Contact Form to send for email</param>
        /// <returns>Task for async operation</returns>
        public async Task SendEmailAsync(string sendto, PostContact form)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(form.From, form.Return));
            message.To.Add(MailboxAddress.Parse(sendto));
            message.Subject = $"[Portfolio Contact] {form.From}";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"Message received from {form.From}: {form.Return}\n\n============\n\n{form.Message}"
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                System.Diagnostics.Debug.Print($"'{_smtpSettings.Server}' => '{_smtpSettings.Username}'");

                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
