using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private StoreDbContext _context;
        public StorageController(StoreDbContext context)
        {
            _context = context;
        }
        
        // private readonly string _bucketName = "sellmarketstorage";
        // private readonly string _projectId = "hopeful-text-427709-g2";
        // private readonly string _serviceAccountKeyPath = "D:\\Rider\\hopeful-text-427709-g2-b8ed2fc88a61.json";
        
    }


}
