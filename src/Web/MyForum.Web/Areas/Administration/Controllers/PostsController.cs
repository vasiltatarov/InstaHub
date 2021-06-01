namespace MyForum.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MyForum.Services.Data;

    public class PostsController : AdministrationController
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.postsService.Delete(id);

            if (!isDeleted)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Posts", "HomePage", new { area = string.Empty });
        }
    }
}
