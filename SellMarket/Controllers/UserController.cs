using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using SellMarket.Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using SellMarket.Model.Mappers;
using SellMarket.Services;

namespace SellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class UserController : Controller
    {
        private StoreDbContext _context;
        readonly IConfiguration _configuration;
        
        private readonly IUserService _userService;

        // private static List<string> _tokenBlacklist = new List<string>();

        public UserController (StoreDbContext context, IConfiguration configuration, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;

        }

        [HttpPost("Register")]
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

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLogin user)
        {
            var userPassword = await _context.Users.FirstOrDefaultAsync(x => x.Password == HashPassword(user.Password));
            var userEmail = await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == user.Email);
            if (userPassword == null) 
            {
                return BadRequest("Passworld incorrect");
            }
            if (userEmail == null)
            {
                return BadRequest("Email incorrect");
            }
            var token = GetAccessToken(user.Email, user.Password);
            return Ok(token);
        }

        
        [HttpPost("GetAccessToken")]
        public string GetAccessToken(string email, string password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email), 
                new Claim(ClaimTypes.Sid, password)
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

        [HttpGet("getUserInfo")]
        [Authorize]
        public List<UserInfoModel> getUserInfo()
        {
            var userEmail = _userService.GetMyEmail();
            var userId = _context.Users.Where(x => x.UserEmail == userEmail).ToList();
            return userId.Select(UserMapper.MapToUserInfoModel).ToList();
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
    }
}
