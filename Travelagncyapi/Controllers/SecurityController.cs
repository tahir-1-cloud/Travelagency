using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travelagncyapi.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travelagncyapi.Dto;

namespace Travelagncyapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly travelContext _dbcontext;
        public SecurityController(travelContext dbconetxt)
        {
            _dbcontext = dbconetxt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Check if email already exists
            var existingUser = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Email already exists in the database" });
            }

            // Create a new User instance
            var user = new User
            {
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
            _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully", Role = user.Role });
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Logindto loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { Message = "All fields are mandatory" });
            }

            // Fetch user from the database
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            // Generate JWT token
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role ?? "Admin"),
        new Claim("UserId", user.Id.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Role = user.Role,
                Id = user.Id
            });
        }

    }

}

