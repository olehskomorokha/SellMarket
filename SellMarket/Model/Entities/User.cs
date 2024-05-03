using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellMarket.Model.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public int UserAdressId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string NickName { get; set; }

        public string UserEmail { get; set; }

        public string PhoneNumber { get; set; }

        // Navigation properties
        public virtual UserAdress UserAdress { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
