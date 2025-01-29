using SellMarket.Model.Entities;
using SellMarket.Model.Models;

namespace SellMarket.Model.Mappers
{
    public class UserMapper
    {
        public static UserRegister MapToUserRegister(User user)
        {
            return new UserRegister
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.UserEmail,
                Password = user.Password
            };
        }

        public static UserInfoModel MapToUserInfoModel(User user)
        {
            return new UserInfoModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                DateOfRegistration = user.DateOfRegistration,
                Address = user.Address,
                UserEmail = user.UserEmail,
                PhoneNumber = user.PhoneNumber
            };
        }
        public static UserLogin MapToUserLogin(User user)
        {
            return new UserLogin
            {
                Email = user.UserEmail,
                Password = user.Password
            };
        }
    }
}
