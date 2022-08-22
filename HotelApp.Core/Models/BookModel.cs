namespace HotelApp.Core.Models
{
    /// <summary>
    /// Model that gets parameters from client
    /// </summary>
    public class BookModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int RoomNumber { get; set; } 
        public DateTime BookForDate { get; set; }
        public DateTime? DateOut { get; set; }
    }
}
