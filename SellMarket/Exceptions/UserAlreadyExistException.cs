using SellMarket.Exeptions;

namespace SellMarket.Exceptions;

public class UserAlreadyExistException : MarketException
{
    public UserAlreadyExistException() 
        : base("alreadyExist", "User with this email already exists")
    {
        
    }
    
}