using booking.Data;
using booking.DTOs.Auth;
using booking.Entities;
using booking.Services.PasswordService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthController(AppDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            bool emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailTaken)
            {
                return BadRequest("Email is already taken.");
            }

            string hashedPassword = _passwordHasher.Hash(dto.Password);

            User user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            bool passwordValid = _passwordHasher.Verify(dto.Password, user.PasswordHash);
            if (passwordValid)
            {
                string token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Invalid email or password.");

            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { user.Id, user.Name, user.Email });
        }
    }
}