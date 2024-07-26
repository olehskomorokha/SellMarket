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
                Img = GetFirstImageUrl(product.ImgURL),
                Description = product.Description,
                SellerName = product.Seller?.NickName,
                Category = product.Category?.Category,
                Price = product.Price
            };
        }
        private static string GetFirstImageUrl(string imgUrls)
        {
            if (string.IsNullOrEmpty(imgUrls)) return string.Empty;
        
            var urls = imgUrls.Split(',');
            return urls.Length > 0 ? urls[0].Trim() : string.Empty;
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
