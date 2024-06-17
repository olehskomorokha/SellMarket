using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using SellMarket.Model.Mappers;
using SellMarket.Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class UserController : Controller
    {
        private StoreDbContext _context;
        readonly IConfiguration _configuration;


        public UserController(StoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpPost]
        public async Task<ActionResult> Register(UserRegister user)
        {
            if (user == null)
            {
                return BadRequest("User data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Missing required user information.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already in use.");
            }

            var hashedPassword = HashPassword(user.Password);

            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                NickName = user.NickName,
                UserEmail = user.Email,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        [HttpPost("GetAccessToken")]
        public string GetAccessToken(string email, int id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email), new Claim(ClaimTypes.Sid, id.ToString())
            };
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(60)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


    }
}
