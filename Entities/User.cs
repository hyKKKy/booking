using booking.Entities.Enums;

namespace booking.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.RegularUser;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
