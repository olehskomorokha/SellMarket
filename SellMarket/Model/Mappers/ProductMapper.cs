using SellMarket.Model.Entities;

namespace SellMarket.Model.Models
{
    public class ProductMapper
    {
        public static ProductInfo MapToProductInfo(Product product)
        {
            return new ProductInfo
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                SellerName = product.Seller?.NickName,
                Category = product.Category?.Category,
                Price = product.Price
            };
        }
        public static ProductCategoryInfo MapToProductCategoryInfo(ProductCategory category)
        {
            return new ProductCategoryInfo
            {
                Id = category.Id,
                Category = category.Category
            };
        }
        
    }
}
