using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using SellMarket.Model.Mappers;
using SellMarket.Model.Models;

namespace SellMarket.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private StoreDbContext _context;

        public UserController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> AddUser(UserRegister user)
        {
            if (user == null)
            {
                return BadRequest("User data is null.");
            }
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) ||
            string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Missing required user information.");
            }
            var newuser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                NickName = user.NickName,
                UserEmail = user.Email,
                Password = user.Password
            };
            _context.Users.Add(newuser);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
