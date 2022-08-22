namespace HotelApp.Core.Models
{
    /// <summary>
    /// Model that gets parameters from client
    /// </summary>
    public class GuestSearchModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
