namespace HotelApp.Data.Entities
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expire { get; set; }
    }
}
