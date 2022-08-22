using System.ComponentModel.DataAnnotations;

namespace HotelApp.Data.Entities
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class Guest : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public Guid? UserId { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
