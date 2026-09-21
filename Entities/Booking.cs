using booking.Entities.Enums;

namespace booking.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;
        public Guid? RoomId { get; set; }
        public Room? Room { get; set; }
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public byte[] RowVersion { get; set; } = [];
    }
}
