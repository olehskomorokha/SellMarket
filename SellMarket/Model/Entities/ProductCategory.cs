using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SellMarket.Model.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; } 
        public string Category {  get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual ProductCategory ParentCategory { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
