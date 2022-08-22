namespace HotelApp.Core.Dto
{
    /// <summary>
    /// DTO for class Guest <see cref="Data.Entities.Guest"/>
    /// </summary>
    public class GuestReadDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
