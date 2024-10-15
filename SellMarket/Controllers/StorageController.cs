﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Mvc;
using SellMarket.Model.Data;

namespace SellMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private const string ApplicationName = "SellMarkedDrive";
        private StoreDbContext _context;
        private readonly DriveService _driveService;
        private string clientId = "839467600642-nerc54ibpgo8bpak15m0pudpsi6jnrjr.apps.googleusercontent.com";
        private string clientSecret = "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";

        private string refresh_token =
            "1//043Dc5-PDLeq-CgYIARAAGAQSNwF-L9Ir9iSO1MvDC85uFBLeVTmEMJ1BMU6rN2LZmJR-is7n9UgU1FXFCGIJg0CY5fOiAS-hHgs";

        private string fileId = "https://drive.google.com/drive/u/0/folders/1gaFnJOev8WcmG61NpnY9YVNxlzGtG_xB";
        
        private DriveService GetDriveService()
        {
            var token = new TokenResponse { RefreshToken = refresh_token };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = Scopes
            });

            var credential = new UserCredential(flow, "user", token);

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }
        public StorageController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var driveService = GetDriveService();
                var fileUrls = new List<string>();
                // Metadata for the file
                for (int i = 0; i < files.Count(); i++)
                {
                    var fileMetadatas = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = files[i].FileName,
                        MimeType = files[i].ContentType
                    };
                    using var streams = files[i].OpenReadStream();
                    var requests = driveService.Files.Create(fileMetadatas, streams, files[i].ContentType);
                    requests.Fields = "id";
                    var progresses = await requests.UploadAsync();
                    if (progresses.Status == UploadStatus.Completed)
                    {
                        var fileId = requests.ResponseBody.Id;

                        // Set the file permission to make it public
                        var permission = new Permission()
                        {
                            Role = "reader",
                            Type = "anyone"
                        };

                        await driveService.Permissions.Create(permission, fileId).ExecuteAsync();

                        // Return the public URL
                        
                        var fileUrl = $"https://drive.google.com/uc?id={fileId}&export";
                        fileUrls.Add(fileUrl);

                    }
                    else
                    {
                        return StatusCode(500, "File upload failed.");
                    }
                    
                }
                // Join the URLs into a single string separated by commas
                string result = string.Join(",", fileUrls);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
        


            // private readonly string _bucketName = "sellmarketstorage";
            // private readonly string _projectId = "hopeful-text-427709-g2";
            // private readonly string _serviceAccountKeyPath = "D:\\Rider\\hopeful-text-427709-g2-b8ed2fc88a61.json";    

}

