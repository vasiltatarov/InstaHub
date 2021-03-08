namespace MyForum.Services.Data
{
    using Microsoft.AspNetCore.Http;

    public interface IUnitOfWork
    {
        void UploadImage(IFormFile file);
    }
}
