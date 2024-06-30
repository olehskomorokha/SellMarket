using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SellMarket.Model.Models;
using SellMarket.Model.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Identity;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private StoreDbContext _context;
        
        // bucket configuring property
        private readonly string _bucketName = "sellmarketstorage";
        private readonly string _projectId = "hopeful-text-427709-g2";
        private readonly string _serviceAccountKeyPath = "D:\\Rider\\hopeful-text-427709-g2-b8ed2fc88a61.json";

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
        [Authorize]
        [HttpPost("addProduct")]
        public async Task<ActionResult> AddProduct([FromForm] AddProductModel productInfo, [FromForm] IFormFileCollection files)
        {
            // image store state
            var storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(_serviceAccountKeyPath));
            var fileUrls = new List<string>();

            foreach (var file in files)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    await storageClient.UploadObjectAsync(_bucketName, uniqueFileName, file.ContentType, stream);
                }   

                var fileUrl = $"https://storage.googleapis.com/{_bucketName}/{uniqueFileName}";
                fileUrls.Add(fileUrl);
            }
            
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            


            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            
            if (user == null)
            {
                return Unauthorized();
            }

            var newProduct = new Product
            {
                Title = productInfo.Title,
                Description = productInfo.Description,
                SellerId = user.Id,
                DateOfPublish = DateTime.Now,
                ImgURL = string.Join(",", fileUrls),
                ProductCategoryId = productInfo.Category,
                Price = productInfo.Price,
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok(newProduct);
            
        }

    }

}
