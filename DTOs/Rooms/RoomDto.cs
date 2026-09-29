namespace booking.DTOs.Rooms
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public required string Number { get; set; }
        public Guid RoomTypeId { get; set; }
    }
}
