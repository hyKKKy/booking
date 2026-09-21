namespace booking.Entities
{
    public class PriceOverride
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Price { get; set; }
    }
}
