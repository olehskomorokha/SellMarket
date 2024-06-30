using System.Security.Claims;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        private readonly string _bucketName = "sellmarketstorage";
        private readonly string _projectId = "hopeful-text-427709-g2";
        private readonly string _serviceAccountKeyPath = "D:\\Rider\\hopeful-text-427709-g2-b8ed2fc88a61.json";
        
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFileCollection files)
        {  
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files received.");
            }
                
            try
            {   
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
                return Ok(new { FileUrls = fileUrls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
