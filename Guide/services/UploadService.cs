using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Services
{
    public class UploadService
    {
        [RequestSizeLimit(209715200)] 
        public async void Upload(string path, string fileName, IFormFile file)
        {
            var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();
        }
    }
}
