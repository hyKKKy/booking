using booking.Infrastructure;
using System.Text.Json.Serialization;

namespace booking.DTOs
{
    public class LoginDto
    {
        public required string Email { get; set; }

        [JsonConverter(typeof(RawStringConverter))]
        public required string Password { get; set; }

    }
}
