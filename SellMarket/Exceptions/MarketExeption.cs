namespace SellMarket.Exeptions;

public class MarketException : Exception
{
    public string Code { get; }

    public MarketException(string code, string message = null, Exception inner = null)
        : base(message ?? code, inner)
    {
        Code = code;
    }
    
}