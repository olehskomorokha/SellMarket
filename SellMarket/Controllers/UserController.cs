using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;

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


    }
}
