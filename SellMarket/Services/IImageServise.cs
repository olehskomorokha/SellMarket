using Microsoft.AspNetCore.Mvc;

namespace SellMarket.Services
{
    public interface IImageServise
    {
        Task<string> UploadFile(IFormFileCollection files);
    }
}

