using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Models;
using Microsoft.AspNetCore.Authorization;
using SellMarket.Services;

namespace SellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class UserController : Controller
    {
        
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegister user)
        {
            return Ok(await _userService.Register(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLogin user)
        {
            return Ok(await _userService.Login(user));
        }

        [HttpGet("getUserInfo")]
        [Authorize]
        public UserInfoModel GetUserInfo()
        {
            return _userService.GetUserInfo();
        }
       
        [HttpPut("addUserAddress")]
        [Authorize]
        public async Task<ActionResult> AddUserAddress(UserContactModel userContactModel)
        {
            return Ok(await _userService.AddUserAddress(userContactModel));
        }
    }
}
