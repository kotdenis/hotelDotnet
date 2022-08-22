namespace HotelApp.Core.Options
{
    /// <summary>
    /// Email settings
    /// </summary>
    public class MailOptions
    {
        public const string MailSettings = "MailSettings";
        public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Security { get; set; } = string.Empty;
    }
}
