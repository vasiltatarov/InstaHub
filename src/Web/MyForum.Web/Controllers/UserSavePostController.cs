namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.UserSavePost;

    [Route("api/[controller]")]
    [ApiController]
    public class UserSavePostController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserSavedPostsService userSavedPostsService;

        public UserSavePostController(UserManager<ApplicationUser> userManager, IUserSavedPostsService userSavedPostsService)
        {
            this.userManager = userManager;
            this.userSavedPostsService = userSavedPostsService;
        }

        [Authorize]
        [HttpGet("{postId}")]
        public async Task<ActionResult<SavePostResponseModel>> Save(int postId)
        {
            var message = "Saved Successfully!";
            var userId = this.userManager.GetUserId(this.User);
            var result = await this.userSavedPostsService.AddAsync(userId, postId);

            if (!result)
            {
                message = "It is already saved!";
            }

            return new SavePostResponseModel() { Message = message };
        }
    }
}
