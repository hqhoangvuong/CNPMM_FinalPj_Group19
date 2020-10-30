using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.Helpers
{
    public class ImageWriter : IImageWriter
    {
        public async Task<string> UploadImage(IFormFile file, string userId)
        {
            if (CheckIfImageFile(file))
            {
                return await WriteFile(file, userId);
            }

            return "Invalid image file";
        }

        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return ImageProcessing.GetImageFormat(fileBytes) != ImageProcessing.ImageFormat.unknown;
        }

        public async Task<string> WriteFile(IFormFile file, string userId)
        {
            string fileName;

            string savePath = "wwwroot\\avatar";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            try
            {
                // var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

                fileName = "avatar_" + userId + ".image"; 

                var path = Path.Combine(Directory.GetCurrentDirectory(), savePath, fileName);
                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                return "An error has been occurred when server try to save the image: " +  e.Message;
            }

            return fileName;
        }
    }
}
