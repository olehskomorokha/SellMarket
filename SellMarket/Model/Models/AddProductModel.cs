namespace SellMarket.Model.Models
{
    public class AddProductModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public int SellerName { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string ImgURL { get; set; }
        public int Category { get; set; }
        public float Price { get; set; }
    }
}
