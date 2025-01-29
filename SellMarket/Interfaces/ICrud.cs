using System.Collections.Generic;
using System.Threading.Tasks;
namespace SellMarket.Services;

public interface ICrud<TModel> where TModel : class
{
    Task<List<TModel>> GetAll();
    Task<TModel> GetById(int id);
    Task<TModel> Create(TModel entity);
    Task Update(TModel entity);
    Task Delete(int id);
}