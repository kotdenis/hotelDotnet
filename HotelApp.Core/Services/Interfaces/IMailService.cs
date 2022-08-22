using HotelApp.Core.Models;

namespace HotelApp.Core.Services.Interfaces
{
    /// <summary>
    /// The contract for MailService <see cref="Implementation.MailService"/>
    /// </summary>
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, CancellationToken ct);
    }
}
