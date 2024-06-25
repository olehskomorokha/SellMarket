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

        [HttpGet("GetProductCategory")]
        public List<ProductCategoryInfo> GetProductCategory()
        {
            //var Email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
            //Console.WriteLine("User with email:"+ Email + " Try to get product category");
            var productCategories = _context.ProductCategories.Where(x => x.ParentCategoryId == null).ToList();
            return productCategories.Select(ProductMapper.MapToProductCategoryInfo).ToList();
        }

        [HttpGet("GetProductsByCategoryId")]
        public List<ProductInfo> GetProductByCategoryId(int Id)
        {
            
            var products = (from p in _context.Products
                    join pc in _context.ProductCategories on p.ProductCategoryId equals pc.Id
                    where pc.ParentCategoryId == Id
                    select new ProductInfo { Title = p.Title, Description = p.Description, SellerName = p.Seller.NickName, Category = pc.Category, Price = p.Price }).ToList();
            return products;
        }
        [HttpGet("GetSubcategoriesByCategoryId")]
        public List<ProductCategoryInfo> GetSubcategoriesByCategoryId(int Id)
        {
            var subcategory = _context.ProductCategories.Where(x => x.ParentCategoryId == Id).ToList();
            return subcategory.Select(ProductMapper.MapToProductCategoryInfo).ToList();
        }
        [HttpGet("GetProductBySubcategoryId")]
        public List<ProductInfo> GetProductByDetailId(int Id)
        {
            var productByDetail = (from p in _context.Products
                                  join Pc in _context.ProductCategories on p.Id equals Pc.Id
                                  where p.ProductCategoryId == Id
                                  select new ProductInfo {Title = p.Title, Description = p.Description, SellerName = p.Seller.NickName, Category = Pc.Category, Price = p.Price}).ToList();

            return productByDetail;
        }

        [HttpPost("addProduct")]
        public Task<ActionResult> AddProduct(ProductInfo productInfo)
        {
            if(productInfo == null)
            {
                return BadRequest("Product not exist");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(productInfo.Category) || string.IsNullOrEmpty(productInfo.Title) ||
                string.IsNullOrEmpty(productInfo.Description)|| string.IsNullOrEmpty(productInfo.SellerName))
            {
                return BadRequest("Missing required product information.");
            }
            
            var newProduct = new Product
            {
                Title = productInfo.Title,
                Description = productInfo.Description,
                Seller = productInfo.SellerName,
                Price = productInfo.Price,

            };
            return Ok(newProduct);
        }

    }
  
}
