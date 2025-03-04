﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SellMarket.Model.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public DateTime DateOfRegistration { get; set; }
        public string Password { get; set; }
        [AllowNull]
        public string? NickName { get; set; }

        public string UserEmail { get; set; }
        [AllowNull]
        public string? Address { get; set; }
        [AllowNull]

        public string? PhoneNumber { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}
