using System.ComponentModel.DataAnnotations;

namespace booking.DTOs.RoomTypes
{
    public class UpdateRoomTypeDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public required string Name { get; set; }

        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10.")]
        public int Capacity { get; set; }

        [Range(typeof(decimal), "0.01", "100000",
            ParseLimitsInInvariantCulture = true,
            ConvertValueInInvariantCulture = true,
            ErrorMessage = "BasePrice must be greater than 0 and less than or equal to 100000.")]
        public decimal BasePrice { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }
    }
}
