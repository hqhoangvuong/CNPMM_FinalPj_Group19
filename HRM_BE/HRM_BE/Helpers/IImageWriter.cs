using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HRM.API.Helpers
{
    public interface IImageWriter
    {
        Task<string> UploadImage(IFormFile file, string userId);
    }
}
