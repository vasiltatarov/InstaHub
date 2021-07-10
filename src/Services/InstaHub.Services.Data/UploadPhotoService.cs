namespace InstaHub.Services.Data
{
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class UploadPhotoService : IUploadPhotoService
    {
        private IHostingEnvironment hostingEnvironment;

        public UploadPhotoService(IHostingEnvironment hostingEnvironment)
            => this.hostingEnvironment = hostingEnvironment;

        public async void UploadImage(IFormFile file)
        {
            var totalBytes = file.Length;
            var fileName = file.FileName.Trim('"');
            fileName = this.EnsureFileName(fileName);
            var buffer = new byte[16 * 1024];

            using (FileStream output = File.Create(this.GetPathAndFileName(fileName)))
            {
                using (Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }
        }

        private string GetPathAndFileName(string fileName)
        {
            var path = this.hostingEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }

        private string EnsureFileName(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            }

            return fileName;
        }
    }
}
