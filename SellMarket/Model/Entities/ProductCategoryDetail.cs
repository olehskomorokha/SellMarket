namespace SellMarket.Model.Entities
{
    public class ProductCategoryDetail
    {
        public int Id { get; set; }
        public string CategoryDetail { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
