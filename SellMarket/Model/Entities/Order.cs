using System.ComponentModel.DataAnnotations.Schema;

namespace SellMarket.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int CustomerId { get; set; }

        public int OrderStateId { get; set; }
        public DateTime OrderDate {  get; set; }

        public User Customer { get; set; }
       
    }
    public enum OrderState
    {
        Open = 1,
        Confirmed = 2,
        Delivered = 3,
        Closed = 4,
    }
}
