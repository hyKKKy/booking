namespace booking.Entities
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<PriceOverride> PriceOverrides { get; set; } = new List<PriceOverride>();
    }
}
