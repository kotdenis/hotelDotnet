using System.ComponentModel.DataAnnotations;

namespace HotelApp.Data.Entities
{
    /// <summary>
    /// Room entity. 
    /// <inheritdoc/>
    /// </summary>
    public class Room : BaseEntity
    {
        /// <summary>
        /// Number of room
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Number of people accepted
        /// </summary>
        [Required]
        public int Capacity { get; set; }
        public decimal PriceForDay { get; set; }
        public bool IsVacant { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
