using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SellMarket.Model.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int SellerId { get; set; }
        [ForeignKey("ProductCategory")]
        public int ProductCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string? ImgURL { get; set; }
        public float Price { get; set; }
        [JsonIgnore]
        public virtual User Seller { get; set; }
        [JsonIgnore]
        public virtual ProductCategory Category { get; set; }
        public virtual ProductDetails Details { get; set; }

    }
}
