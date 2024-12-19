using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SellMarket.Services
{
    public class ImageService : IImageService
    {
        public ImageService(IConfiguration configuration)
        {
            _clientId = configuration["GoogleDriveApi:ClientId"];
            _clientSecret = configuration["GoogleDriveApi:ClientSecret"];
            _refresh_token = configuration["GoogleDriveApi:refresh_token"];
            _folderId = configuration["GoogleDriveApi:folderId"];
        }
        private string _clientId;
        private string _clientSecret;
        private string _refresh_token;
        private string _folderId;
        private readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private const string ApplicationName = "SellMarkedDrive";

        private DriveService GetDriveService()
        {
            var token = new TokenResponse { RefreshToken = _refresh_token };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _clientId,
                    ClientSecret = _clientSecret
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
        public async Task<string> UploadFile(IFormFileCollection files)
        {
            var fileId = string.Empty;
            if (files == null || files.Count == 0)
            {
                return "No file uploaded.";
            }

            try
            {
                var driveService = GetDriveService();
                var fileUrls = new List<string>();
                // Metadata for the file
                foreach (var file in files)
                {
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = file.FileName,
                        MimeType = file.ContentType,
                        Parents = new List<string> { _folderId }
                    };

                    using var stream = file.OpenReadStream();
                    var request = driveService.Files.Create(fileMetadata, stream, file.ContentType);
                    request.Fields = "id";

                    var progress = await request.UploadAsync();
                    if (progress.Status == UploadStatus.Completed)
                    {
                        fileId = request.ResponseBody.Id;

                        // Set the file permission to make it public
                        var permission = new Permission()
                        {
                            Role = "reader",
                            Type = "anyone"
                        };
                        await driveService.Permissions.Create(permission, fileId).ExecuteAsync();

                        // Construct the public URL
                        var fileUrl = $"https://drive.google.com/thumbnail?id={fileId}";
                        fileUrls.Add(fileUrl);
                    }
                    else
                    {
                        throw progress.Exception;
                    }
                }
                // Join the URLs into a single string separated by commas
                return string.Join(",", fileUrls); ;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}