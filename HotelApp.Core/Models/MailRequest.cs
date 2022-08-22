namespace HotelApp.Core.Models
{
    /// <summary>
    /// Email getter parameters
    /// </summary>
    public class MailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
