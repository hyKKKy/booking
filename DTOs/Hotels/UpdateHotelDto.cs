using System.ComponentModel.DataAnnotations;

namespace booking.DTOs.Hotels
{
    public class UpdateHotelDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [MinLength(5, ErrorMessage = "Address must be at least 5 characters long.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "City must be at least 2 characters long.")]
        public required string City { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
