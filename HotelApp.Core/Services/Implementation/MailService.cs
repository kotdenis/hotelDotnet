using HotelApp.Core.Models;
using HotelApp.Core.Options;
using HotelApp.Core.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HotelApp.Core.Services.Implementation
{
    /// <summary>
    /// Implements methods for email sending
    /// </summary>
    public class MailService : IMailService
    {
        private readonly MailOptions _mailOptions;
        public MailService(IOptions<MailOptions> mailOptions)
        {
            _mailOptions = mailOptions.Value;
        }

        /// <summary>
        /// Sends email
        /// </summary>
        /// <param name="mailRequest"><see cref="MailRequest"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken ct)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailOptions.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailOptions.Mail, _mailOptions.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
