namespace HotelApp.Core.Models
{
    /// <summary>
    /// Model that gets parameters from client
    /// </summary>
    public class RoomSearchModel
    {
        public int Capacity { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
