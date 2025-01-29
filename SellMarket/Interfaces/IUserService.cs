using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Entities;
using SellMarket.Model.Models;

namespace SellMarket.Services
{
    public interface IUserService
    {
        Task<string> Login(UserLogin user);
        Task<UserRegister> Register(UserRegister user);
        Task<User> AddUserAddress(UserContactModel userContactModel);
        UserInfoModel GetUserInfo();
        string GetMyEmail();
    }
}
