
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        public StorageController(ILogger<StorageController> logger)
        {
            _logger = logger;
        }
        
        private readonly string _bucketName = "sellmarketstorage";
        private readonly string _projectId = "hopeful-text-427709-g2";
        private readonly string _serviceAccountKeyPath = "D:\\Rider\\hopeful-text-427709-g2-b8ed2fc88a61.json";

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }

            try
            {
                // Initialize the Google Cloud Storage client
                var storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(_serviceAccountKeyPath));
            
                // Generate a unique name for the file to avoid overwriting
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Upload the file to Google Cloud Storage
                using (var stream = file.OpenReadStream())
                {
                    await storageClient.UploadObjectAsync(_bucketName, uniqueFileName, "image/png", stream);
                }

                return Content("File uploaded successfully: " + uniqueFileName);
            }
            catch (Exception ex)
            {
                // Handle errors
                return Content("An error occurred: " + ex.Message);
            }
        }
    }
}
