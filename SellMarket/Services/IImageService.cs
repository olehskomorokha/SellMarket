namespace SellMarket.Services
{
    
    public interface IImageService
    {
        Task<string> UploadFile(IFormFileCollection files);
    }
}
