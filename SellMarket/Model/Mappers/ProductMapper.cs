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
                DateOfPublish = product.DateOfPublish,
                SellerName = product.Seller?.NickName,
                Category = product.Category?.Category,
                Price = product.Price
            };
        }
        public static ProductInfo MapToAllProductInfo(Product product, User? user, ProductCategory category)
        {
            return new ProductInfo
            {
                Id = product.Id,
                Title = product.Title,
                Img = GetFirstImageUrl(product.ImgURL),
                Description = product.Description,
                SellerName = user.NickName, // Use the User object to get the seller's name
                Category = category.Category, // Use the ProductCategory object to get the category
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

        public static AllProductInfoModel MapToAllProductInfoModel(Product product, User? user, ProductCategory category)
        {
            return new AllProductInfoModel
            {
                Id = product.Id,
                Title = product.Title,
                Img = GetFirstImageUrl(product.ImgURL),
                DateOfPublish = product.DateOfPublish,
                Description = product.Description,
                SellerName = user.NickName, 
                Category = category.Category,
                Price = product.Price
            };
        }
    }
}
