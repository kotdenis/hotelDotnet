namespace HotelApp.Core.Dto
{
    /// <summary>
    /// DTO for class Room <see cref="HotelApp.Data.Entities.Room"/>
    /// </summary>
    public class RoomReadDto
    {
        public int Number { get; set; }
        public int Capacity { get; set; }
        public decimal PriceForDay { get; set; }
    }
}
