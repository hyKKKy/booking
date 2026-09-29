using System.ComponentModel.DataAnnotations;

namespace booking.DTOs
{
    public class CreateRoomDto
    {
        [Required(ErrorMessage = "Number is required.")]
        [StringLength(6, ErrorMessage = "Number cannot exceed 6 characters.")]
        public required string Number { get; set; }
        public required Guid RoomTypeId { get; set; }
    }
}
