using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SellMarket.Model.Models;
using SellMarket.Model.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private StoreDbContext _context;

        public ProductController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllProduct")]
        public List<ProductInfo> GetAll()
        {
            var products = _context.Products.Include(x => x.Seller).Include(x => x.Category).ToList();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }

        [HttpGet("GetAllProductById")]
        [ProducesResponseType(typeof(Product), 200)] // Specify the expected response type
        public List<ProductInfo> GetAllProduct(int Id)
        {
            var products = _context.Products.Include(x => x.Seller).Include(x => x.Category).Where(p => p.ProductCategoryId == Id).ToList();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }

        [Authorize]
        [HttpGet("GetProductCategory")]
        public List<ProductCategoryInfo> GetProductCategory()
        {
            var Email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
            Console.WriteLine("User with email:"+ Email + " Try to get product category");
            var productCategories = _context.ProductCategories.Where(x => x.ParentCategoryId == null).ToList();
            return productCategories.Select(ProductMapper.MapToProductCategoryInfo).ToList();
        }

        [HttpGet("GetProductCategoryDetail")]
        public List<ProductCategoryDetailInfo> GetProductCategoryDetail(int Id)
        {
            var productCategoriDetail = _context.ProductCategotyDetail.Include(x => x.ProductCategory).Where(x => x.ProductCategoryId == Id).ToList();
            return productCategoriDetail.Select(ProductMapper.MapToProductCategoryDetailInfo).ToList();
        }
        [HttpGet("GetProductByDetailId")]
        public List<ProductInfo> GetProductByDetailId(int Id)
        {
            var productByDetail = (from p in _context.Products
                                  join Pc in _context.ProductCategories on p.Id equals Pc.Id
                                  join PcD in _context.ProductCategotyDetail on Pc.Id equals PcD.Id
                                  where PcD.Id == Id
                                  select new ProductInfo {Title = p.Title, Description = p.Description, SellerName = p.Seller.NickName, Category = Pc.Category, Price = p.Price}).ToList();

            var products = _context.Products.Include(x => x.Category).Where(x => x.ProductCategoryId == Id).ToList();
            return productByDetail;
        }




    }
  
}
