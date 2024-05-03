using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellMarket.Model.Entities
{
    public class UserAdress
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }
}
