namespace booking.DTOs
{
    public class HotelDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public string? Description { get; set; }
    }
}
