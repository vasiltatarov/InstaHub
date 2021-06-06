namespace MyForum.Services.Data
{
    using Microsoft.AspNetCore.Http;

    public interface IUploadPhotoService
    {
        void UploadImage(IFormFile file);
    }
}
