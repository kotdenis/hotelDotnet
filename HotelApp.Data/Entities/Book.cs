namespace HotelApp.Data.Entities
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class Book : BaseEntity
    {
        /// <summary>
        /// The date of incoming
        /// </summary>
        public DateTime? DateBegin { get; set; }
        /// <summary>
        /// The date of outgoing
        /// </summary>
        public DateTime? DateEnd { get; set; }
        public Guid RoomId { get; set; }
        public Guid GuestId { get; set; }


        public Room Room { get; set; } = new Room();
        public Guest Guest { get; set; } = new Guest();
    }
}
