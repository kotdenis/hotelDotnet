namespace HotelApp.Core.Models
{
    /// <summary>
    /// Model to return to client
    /// </summary>
    public class TokenModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
