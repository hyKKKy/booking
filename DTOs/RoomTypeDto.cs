namespace booking.DTOs
{
    public class RoomTypeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public Guid HotelId { get; set; }
    }
}
