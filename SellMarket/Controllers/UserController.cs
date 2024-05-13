using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
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

        [HttpPost("AddUser")]
        public void AddUser(string Name, string LastName,string PassWorld, string Email, string PhoneNumber, string Nick)
        {
            var user = _context.Users.Add(new User {UserAdressId = 5, FirstName = Name, LastName = LastName,Password = PassWorld, UserEmail = Email, PhoneNumber = PhoneNumber, NickName = Nick});
            _context.SaveChanges();
        }
    }
}
