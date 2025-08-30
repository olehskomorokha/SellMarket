using Google.Apis.Drive.v3.Data;
using SellMarket.Exeptions;

namespace SellMarket.Exceptions;

public class UserNotFoundException : MarketException
{
    public UserNotFoundException() 
        : base("notFound", "User not found")
    {
    }
}