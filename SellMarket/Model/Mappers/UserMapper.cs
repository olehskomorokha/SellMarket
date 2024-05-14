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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.UserEmail,
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
