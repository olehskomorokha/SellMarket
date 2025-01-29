namespace SellMarket.Exeptions;

public class MarketExeption : Exception
{
    public MarketExeption() { }
    public MarketExeption(string message) : base(message) { }
    public MarketExeption(string message, Exception inner) : base(message, inner) { }
    
}