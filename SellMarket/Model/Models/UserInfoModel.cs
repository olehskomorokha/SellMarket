namespace SellMarket.Model.Models;

public class UserInfoModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string NickName { get; set; }
    public DateTime DateOfRegistration { get; set; }
    public string UserEmail { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    
    // address
    // public string Region { get; set; }
    // public string City { get; set; }
}