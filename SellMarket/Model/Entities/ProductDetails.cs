using System.ComponentModel.DataAnnotations.Schema;

namespace SellMarket.Model.Entities
{
    public class ProductDetails
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public string CompanyName { get; set; }

        public Product Product { get; set; }

    }
}
