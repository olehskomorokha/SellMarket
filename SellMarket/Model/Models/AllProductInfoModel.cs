﻿namespace SellMarket.Model.Models
{
    public class AllProductInfoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string Description { get; set; } 
        public string? SellerName { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
    }
}