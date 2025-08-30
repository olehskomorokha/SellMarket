using SellMarket.Exeptions;

namespace SellMarket.Exceptions;

public class LoginException : MarketException
{
    public LoginException(string code, string message) 
        : base(code, message)
    {
        
    }
    
}