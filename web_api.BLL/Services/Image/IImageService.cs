using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string directoryPath);
        void DeleteImage(string filePath);
        void CreateDirectory(string path);
        Task<List<CarImage>> SaveCarImagesAsync(IEnumerable<IFormFile> images, string directoryPath);
    }
}
