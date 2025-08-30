using System.Security.Claims;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellMarket.Exceptions;
using SellMarket.Exeptions;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using SellMarket.Model.Models;

namespace SellMarket.Services;

public class ProductService : IProductService
{
    private readonly StoreDbContext _context;
    private readonly IImageService _imageService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public ProductService(StoreDbContext context, IImageService imageService, IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _imageService = imageService;
        _context = context;
        _configuration = configuration;
    }
    // Crud
    public async Task<List<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            throw new MarketException("user not found");
        }
        return product;
    }

    public async Task<Product> Create(Product product)
    {
        var newProduct = await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return newProduct.Entity;
    }

    public async Task Update(Product product)
    {
        if (string.IsNullOrEmpty(product.Title)) throw new MarketException("title is empty");
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int modelId)
    {
        var product = await _context.Products.FindAsync(modelId);
        if (product != null)
        {
            await _imageService.DeleteFile(product.ImgURL);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ProductNotFoundException();
        }
    }
    // /Crud
    
    public async Task<List<ProductInfo>> GetAllProductInfo()
    {
        var products = await _context.Products.Include(x => x.Seller).Include(x => x.Category).ToListAsync();
        return products.Select(ProductMapper.MapToProductInfo).ToList();
    }

    public async Task<List<ProductInfo>> GetAllProductsBySubcategoryId(int id)
    {
        var products = await _context.Products.Include(x => x.Seller).Include(x => x.Category).Where(p => p.ProductCategoryId == id).ToListAsync();
        return products.Select(ProductMapper.MapToProductInfo).ToList();
    }

    public async Task<List<ProductCategoryInfo>> GetProductCategory()
    {
        var productCategories = await _context.ProductCategories.Where(x => x.ParentCategoryId == null || x.ParentCategoryId == x.Id).ToListAsync();
        return productCategories.Select(ProductMapper.MapToProductCategoryInfo).ToList();
    }

    public async Task<List<ProductCategoryInfo>> GetAllSubcategory()
    {
        var subcategory = await _context.ProductCategories.Where(x => x.ParentCategoryId != x.Id && x.ParentCategoryId != null).ToListAsync();
        return subcategory.Select(ProductMapper.MapToProductCategoryInfo).ToList();
    }

    public async Task<List<ProductCategoryInfo>> GetSubcategoriesByCategoryId(int id)
    {
        var subcategory = await _context.ProductCategories.Where(x => x.ParentCategoryId == id && x.ParentCategoryId != x.Id && x.ParentCategoryId != null).ToListAsync();
        return subcategory.Select(ProductMapper.MapToProductCategoryInfo).ToList();
    }

    public async Task<Product> Create(AddProductModel productInfo, IFormFileCollection files)
    {
        var imgUrls = await _imageService.UploadFile(files);
        var email = _userService.GetMyEmail();

        if (string.IsNullOrEmpty(email))
        {
            throw new IsNullOrEmptyException("inNull", "Email is empty.");
        }
                
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
                
        if (user == null)
        {
            throw new ArgumentNullException();
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

        return newProduct;
    }

    public async Task<List<ProductInfo>> GetProductByTitle(string keyWord)
    {
        var products = await _context.Products.Where(x => x.Title.Contains(keyWord)).ToListAsync();
        return products.Select(ProductMapper.MapToProductInfo).ToList();
    }

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
                query = query.OrderBy(x => x.Id);
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

    public async Task<IEnumerable<string>> GetProductImg(int productId)
    {
        var images = await _context.Products.Where(x => x.Id == productId).ToListAsync();
        return images.Select(x => x.ImgURL);
    }

    public async Task<List<ProductInfo>> GetNewProduct()
    {
        var product = await _context.Products.OrderBy(x => x.DateOfPublish).ToListAsync();
        return product.Select(ProductMapper.MapToProductInfo).ToList();
    }

    public async Task<List<ProductInfo>> GetUserPosts()
    {
        var userEmail = _userService.GetMyEmail();  
        Console.WriteLine(userEmail);
        var userId = _context.Users.Where(x => x.UserEmail == userEmail).Select(x => x.Id).FirstOrDefault();
        var product = await _context.Products.Where(x => x.SellerId == userId).ToListAsync();
        return product.Select(ProductMapper.MapToProductInfo).ToList();
    }

    public async void DeleteProduct(int productId)
    {
        try
        {
            var bucketName = _configuration["GoogleCloud:BucketName"];
            var serviceAccountKeyPath = _configuration["GoogleCloud:ServiceAccountKeyPath"];
            
            var storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(serviceAccountKeyPath));
            var imageUrl = await _context.Products.Where(x => x.Id == productId).Select(x => x.ImgURL).FirstOrDefaultAsync();
            var imgArray = imageUrl.Split(",");
            foreach (var img in imgArray)
            {
                storageClient.DeleteObject(bucketName, Path.GetFileName(img));
            }

            var product = await _context.Products.Where(x => x.Id == productId).FirstAsync();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}