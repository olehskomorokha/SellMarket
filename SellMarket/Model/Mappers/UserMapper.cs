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
                PhoneNumber = user.PhoneNumber,
                NickName = user.NickName,
                Email = user.UserEmail,
                Password = user.Password
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
