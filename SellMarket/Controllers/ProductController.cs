using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Entities;
using SellMarket.Model.Models;
using SellMarket.Services;
using Microsoft.AspNetCore.Authorization;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAllProduct")]
        public async Task<List<ProductInfo>> GetAll()
        {
            return await _productService.GetAllProductInfo();
        }

        [HttpGet("GetProductsBySubcategoryId")]
        [ProducesResponseType(typeof(Product), 200)]
        public async Task<List<ProductInfo>> GetAllProductsBySubcategoryId(int id)
        {
            return await _productService.GetAllProductsBySubcategoryId(id);
        }

        [HttpGet("GetProductCategory")]
        public async Task<List<ProductCategoryInfo>> GetProductCategory()
        {
            return await _productService.GetProductCategory();
        }  

        [HttpGet("GetAllSubcategory")]
        public async Task<List<ProductCategoryInfo>> GetAllSubcategory()
        {
            return await _productService.GetAllSubcategory();
        }
        [HttpGet("GetSubcategoriesByCategoryId")]
        public async Task<List<ProductCategoryInfo>> GetSubcategoriesByCategoryId(int id)
        {
            return await _productService.GetSubcategoriesByCategoryId(id);
        }
        [HttpPost("addProduct")]
        [Authorize] 
        public async Task<ActionResult> AddProduct([FromForm] AddProductModel productInfo, [FromForm] IFormFileCollection files)
        {
            return Ok(await _productService.Create(productInfo, files));
        }
        [HttpGet("GetProductByTitle")]
        public async Task<List<ProductInfo>> GetProductByTitle(string keyWord)
        {
            return await _productService.GetProductByTitle(keyWord);
        }

        [HttpGet("GetProductsById")]
        public async Task<List<AllProductInfoModel>> GetProductById(int id)
        {
            return await _productService.GetProductById(id);
        }
        [HttpGet("GetProductsBySubcategoryWithFilterId")]
        public async Task<List<ProductInfo>> GetAllProductBySubcategoryWithFilterId(int id, int? minPrice, int? maxPrice, string? sortType)
        {
            return await _productService.GetAllProductBySubcategoryWithFilterId(id, minPrice, maxPrice, sortType);
        }
        
        [HttpGet("GetProductImg")]
        public async Task<IEnumerable<string>> GetProductImg(int productId)
        {
            return await _productService.GetProductImg(productId);
        }

        [HttpGet("GetNewProduct")]
        public async Task<List<ProductInfo>> GetNewProduct()
        {
            return await _productService.GetNewProduct();
        }

        [HttpGet("GetUserPosts")]
        [Authorize]
        public async Task<List<ProductInfo>> GetUserPosts()
        {
            return await _productService.GetUserPosts();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(Product product)
        {
            await _productService.Update(product);
            return Ok();
        }
        

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize]
        public async  Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.Delete(id);
            return Ok();
        }
    }

}   
