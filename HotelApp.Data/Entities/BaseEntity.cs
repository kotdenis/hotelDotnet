namespace HotelApp.Data.Entities
{
    /// <summary>
    /// Implements properties for all entities
    /// </summary>
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
