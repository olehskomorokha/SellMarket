using SellMarket.Exeptions;

namespace SellMarket.Exceptions;

public class ProductNotFoundException : MarketException
{
    public ProductNotFoundException()
        : base("notFound", "Product not found")
    {
        
    }
}