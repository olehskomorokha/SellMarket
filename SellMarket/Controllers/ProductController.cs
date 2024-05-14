using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SellMarket.Model.Models;
using SellMarket.Model.Mappers;

namespace SellMarket.Controllers
{
    [ApiController]
    public class ProductController : Controller
    {
        private StoreDbContext _context;

        public ProductController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Product), 200)] // Specify the expected response type
        public List<ProductInfo> GetAllProduct(int Id, string keyword = "")
        {
            var products = _context.Products.Include(x => x.Seller).Include(x => x.Category).Where(p => p.ProductCategoryId == Id).ToList();
            if(keyword == "byPriceDes")
            {
                products = products.OrderByDescending(x => x.Price).ToList();
            }
            else if (keyword == "byPriceAce")
            {
                products = products.OrderBy(x => x.Price).ToList();
            }

            //return products.Select(x => new ProductInfo(){Id = x.Id, Category = x.Category?.Category, SellerName = x.Seller?.NickName, Description = x.Description, Title = x.Title, Price = x.Price}).ToList();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }



        
    }
  
}
