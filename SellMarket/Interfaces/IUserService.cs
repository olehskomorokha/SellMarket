using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Entities;
using SellMarket.Model.Models;

namespace SellMarket.Services
{
    public interface IUserService : ICrud<User>
    {
        Task<string> Login(UserLogin user);
        Task<UserRegister> Register(UserRegister user);
        Task<User> AddUserAddress(UserContactModel userContactModel);
        Task UpdateUserModel(UpdateUserSettingsModel userModel);
        Task<UserInfoModel> GetUserInfo();
        string GetMyEmail();
    }
}
