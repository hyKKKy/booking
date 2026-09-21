namespace booking.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public string? Description { get; set; }
        public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();

    }
}
