using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Entities;
using SellMarket.Model.Models;

namespace SellMarket.Services;

public interface IProductService : ICrud<Product>
{
    Task<List<ProductInfo>> GetAllProductInfo();
    Task<List<ProductInfo>> GetAllProductsBySubcategoryId(int id);
    Task<List<ProductCategoryInfo>> GetProductCategory();
    Task<List<ProductCategoryInfo>> GetAllSubcategory();
    Task<List<ProductCategoryInfo>> GetSubcategoriesByCategoryId(int id);
    Task<Product> Create([FromForm] AddProductModel productInfo, [FromForm] IFormFileCollection files);
    Task<List<ProductInfo>> GetProductByTitle(string keyWord);
    Task<List<AllProductInfoModel>> GetProductById(int id);

    Task<List<ProductInfo>> GetAllProductBySubcategoryWithFilterId(int id, int? minPrice, int? maxPrice,
        string? sortType);

    Task<IEnumerable<string>> GetProductImg(int productId);
    Task<List<ProductInfo>> GetNewProduct();
    Task<List<ProductInfo>> GetUserPosts();
    void DeleteProduct(int productId);
}