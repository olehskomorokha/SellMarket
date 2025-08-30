using SellMarket.Exeptions;

namespace SellMarket.Exceptions;

public class IsNullOrEmptyException : MarketException
{
    public IsNullOrEmptyException(string code, string message) 
        : base(code, message)
    {
        
    }
}