namespace HotelApp.Core.Models
{
    /// <summary>
    /// Model that gets parameters from client
    /// </summary>
    public class LoginModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
