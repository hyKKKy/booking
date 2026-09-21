using Microsoft.EntityFrameworkCore;
using booking.Entities;

namespace booking.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<PriceOverride> PriceOverrides => Set<PriceOverride>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<RoomType>()
                .HasIndex(rt => new { rt.HotelId, rt.Name })
                .IsUnique();
            modelBuilder.Entity<PriceOverride>()
                .HasIndex(po => new { po.RoomTypeId, po.Date })
                .IsUnique();
            modelBuilder.Entity<RoomType>()
                .Property(rt => rt.BasePrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Booking>()
                .Property(b => b.RowVersion)
                .IsRowVersion();
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<PriceOverride>()
                .Property(po => po.Price)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.RoomTypeId, b.CheckIn, b.CheckOut });

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)          // у брони один User
                .WithMany(u => u.Bookings)    // у юзера много Bookings
                .HasForeignKey(b => b.UserId) // через этот FK
                .OnDelete(DeleteBehavior.Restrict);  // поведение при удалении
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RoomType)      // у брони один RoomType
                .WithMany(rt => rt.Bookings)  // у RoomType много Bookings
                .HasForeignKey(b => b.RoomTypeId) // через этот FK
                .OnDelete(DeleteBehavior.Restrict);  // поведение при удалении
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)          // у брони один Room
                .WithMany(r => r.Bookings)    // у Room много Bookings
                .HasForeignKey(b => b.RoomId) // через этот FK
                .OnDelete(DeleteBehavior.Restrict);  // поведение при удалении

        }

    }
}
