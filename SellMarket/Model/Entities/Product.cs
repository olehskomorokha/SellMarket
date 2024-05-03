using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int ProductAmount { get; set; }

        public float Price { get; set; }

        public virtual User Seller { get; set; }
        public virtual ProductCategory Category { get; set; }
        public virtual ProductDetails Details { get; set; }




    }
}
