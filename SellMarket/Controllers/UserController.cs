using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Models;
using Microsoft.AspNetCore.Authorization;
using SellMarket.Model.Entities;
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

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _userService.GetById(id));
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
        public async Task<ActionResult<UserInfoModel>> GetUserInfo()
        {
            return Ok(await _userService.GetUserInfo());
        }

        [HttpGet("getMyEmail")]
        [Authorize]
        public string GetMyEmail()
        {
            return _userService.GetMyEmail();
        }
        [HttpPut("addUserAddress")]
        [Authorize]
        public async Task<ActionResult> AddUserAddress(UserContactModel userContactModel)
        {
            return Ok(await _userService.AddUserAddress(userContactModel));
        }

        [HttpPut("UpdateUserSettings")]
        [Authorize] 
        public async Task<ActionResult> Update(UpdateUserSettingsModel user)
        {
            await _userService.UpdateUserModel(user);
            return Ok();
        }
    }
}
