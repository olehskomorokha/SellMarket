using System.Text.Json.Serialization;

namespace SellMarket.Model.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; } 
        public string Category {  get; set; }
        


        public ICollection<ProductCategoryDetail> ProductCategoryDetail { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
