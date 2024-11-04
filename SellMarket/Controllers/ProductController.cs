using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using Microsoft.EntityFrameworkCore;
using SellMarket.Model.Models;
using SellMarket.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Google.Cloud.Storage.V1;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private StoreDbContext _context;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        
        // bucket configuring property
        private readonly string _bucketName = "sellmarketstorage";
        // private readonly string _projectId = "hopeful-text-427709-g2";
        private readonly string _serviceAccountKeyPath = "D:\\Rider\\hopeful-text-427709-g2-b8ed2fc88a61.json";

        public ProductController(StoreDbContext context, IUserService userService, IImageService imageService)
        {
            _context = context;
            _userService = userService;
            _imageService = imageService;
        }
        [HttpGet("GetAllProduct")]
        public async Task<List<ProductInfo>> GetAll()
        {
            var products = await _context.Products.Include(x => x.Seller).Include(x => x.Category).ToListAsync();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }

        [HttpGet("GetAllProductBySubcategoryId")]
        [ProducesResponseType(typeof(Product), 200)] // Specify the expected response type
        public async Task<List<ProductInfo>> GetAllProductBySubcategoryId(int id)
        {
            var products = await _context.Products.Include(x => x.Seller).Include(x => x.Category).Where(p => p.ProductCategoryId == id).ToListAsync();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }

        [HttpGet("GetProductCategory")]
        public async Task<List<ProductCategoryInfo>> GetProductCategory()
        {
            // var Email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
            // Console.WriteLine("User with email:"+ Email + " Try to get product category");
            var productCategories = await _context.ProductCategories.Where(x => x.ParentCategoryId == null).ToListAsync();
            return productCategories.Select(ProductMapper.MapToProductCategoryInfo).ToList();
        }   

        [HttpGet("GetProductsByCategoryId")]
        public async Task<List<ProductInfo>> GetProductByCategoryId(int id)
        {
            var products = await (from p in _context.Products
                join pc in _context.ProductCategories on p.ProductCategoryId equals pc.Id
                where pc.ParentCategoryId == id
                select new ProductInfo 
                { 
                    Title = p.Title, 
                    Description = p.Description, 
                    SellerName = p.Seller.NickName, 
                    Category = pc.Category, 
                    Price = p.Price 
                }).ToListAsync();

            return products;
        }

        [HttpGet("GetAllSubcategory")]
        public async Task<List<ProductCategoryInfo>> GetAllSubcategory()
        {
            var subcategory = await _context.ProductCategories.Where(x => x.ParentCategoryId != null).ToListAsync();
            return subcategory.Select(ProductMapper.MapToProductCategoryInfo).ToList();
        }
        [HttpGet("GetSubcategoriesByCategoryId")]
        public async Task<List<ProductCategoryInfo>> GetSubcategoriesByCategoryId(int id)
        {
            var subcategory = await _context.ProductCategories.Where(x => x.ParentCategoryId == id).ToListAsync();
            return subcategory.Select(ProductMapper.MapToProductCategoryInfo).ToList();
        }
        
        
        [HttpPost("addProduct")]
        [Authorize]
        public async Task<ActionResult> AddProduct([FromForm] AddProductModel productInfo, [FromForm] IFormFileCollection files)
        {
            // // image store state
            // var storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(_serviceAccountKeyPath));
            // var fileUrls = new List<string>();
            //
            // foreach (var file in files)
            // {
            //     var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            //     using (var stream = file.OpenReadStream())
            //     {
            //         await storageClient.UploadObjectAsync(_bucketName, uniqueFileName, file.ContentType, stream);
            //     }   
            //
            //     var fileUrl = $"https://storage.googleapis.com/{_bucketName}/{uniqueFileName}";
            //     fileUrls.Add(fileUrl);
            // }
            //
            string imgUrls = await _imageService.UploadFile(files);
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
                ImgURL =  imgUrls,
                ProductCategoryId = productInfo.Category,
                Price = productInfo.Price,
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok(newProduct);
        }

        [HttpGet("GetProductByTitle")]
        public async Task<List<ProductInfo>> GetProductByTitle(string keyWord)
        {
            var products = await _context.Products.Where(x => x.Title.Contains(keyWord)).ToListAsync();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }
        [HttpGet("GetProducts")]
        public async Task<List<AllProductInfoModel>> GetProducts()
        {
            var product = await (from p in _context.Products
                join pc in _context.ProductCategories on p.ProductCategoryId equals pc.Id
                join s in _context.Users on p.SellerId equals s.Id
                select new
                {
                    Product = p,
                    User = s,
                    Category = pc
                }).ToListAsync();
            return product.Select(p => ProductMapper.MapToAllProductInfoModel(p.Product, p.User, p.Category)).ToList();
        }

        [HttpGet("GetProductById")]
        public async Task<List<AllProductInfoModel>> GetProductById(int id)
        {
            var product = await (from p in _context.Products
                join pc in _context.ProductCategories on p.ProductCategoryId equals pc.Id
                join s in _context.Users on p.SellerId equals s.Id
                where p.Id == id
                select new
                {
                    Product = p,
                    User = s,
                    Category = pc
                }).ToListAsync();
            return product.Select(p => ProductMapper.MapToAllProductInfoModel(p.Product, p.User, p.Category)).ToList();
        }
        [HttpGet("GetAllProductBySubcategoryWithFilterId")]
        public async Task<List<ProductInfo>> GetAllProductBySubcategoryWithFilterId(int id, int? minPrice, int? maxPrice, string? sortType)
        {
            var query = _context.Products.Include(x => x.Category).AsQueryable();
            switch (sortType)
            {
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "-price":
                    query = query.OrderByDescending(x => x.Price);  
                    break;
                case "-date_created":
                    query = query.OrderByDescending(x => x.DateOfPublish);
                    break;
                default:
                    query = query.OrderBy(x => x.Id); // Assuming "position" means the order of ID or a default order
                    break;
            }
            query = query.Where(p => p.ProductCategoryId == id);
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price > minPrice);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price < maxPrice);
            }
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(p => p.Price > minPrice && p.Price < maxPrice);
            }
            
            var products = await query.ToListAsync();
            return products.Select(ProductMapper.MapToProductInfo).ToList();
        }
        
        [HttpGet("GetProductImg")]
        public async Task<IEnumerable<string>> GetProductImg(int productId)
        {
            var images = await _context.Products.Where(x => x.Id == productId).ToListAsync();
            return images.Select(x => x.ImgURL);
        }

        [HttpGet("GetNewProduct")]
        public async Task<List<ProductInfo>> GetNewProduct()
        {
            var product = await _context.Products.OrderBy(x => x.DateOfPublish).ToListAsync();
            
            return product.Select(ProductMapper.MapToProductInfo).ToList();
        }

        [HttpGet("GetUserPosts")]
        [Authorize]
        public async Task<List<ProductInfo>> GetUserPosts()
        {
            var userEmail = _userService.GetMyEmail();  
            Console.WriteLine(userEmail);
            var userId = _context.Users.Where(x => x.UserEmail == userEmail).Select(x => x.Id).FirstOrDefault();
            var product = await _context.Products.Where(x => x.SellerId == userId).ToListAsync();
            return product.Select(ProductMapper.MapToProductInfo).ToList();
        }

        [HttpDelete("DeleteProduct")]
        [Authorize]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(_serviceAccountKeyPath));
            var imageUrl = await _context.Products.Where(x => x.Id == productId).Select(x => x.ImgURL).FirstOrDefaultAsync();
            var imgArray = imageUrl.Split(",");
            foreach (var img in imgArray)
            {
                storageClient.DeleteObject(_bucketName, Path.GetFileName(img));
            }

            var product = await _context.Products.Where(x => x.Id == productId).FirstAsync();
            _context.Products.Remove(product);
            _context.SaveChangesAsync();
            return Ok(product);
        }
    }

}   
