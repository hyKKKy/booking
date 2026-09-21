namespace booking.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public required string Number { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
