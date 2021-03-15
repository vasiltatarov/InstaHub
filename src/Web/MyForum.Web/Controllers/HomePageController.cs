namespace MyForum.Web.Controllers
{
    using System.Globalization;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.Posts;

    [Authorize]
    public class HomePageController : Controller
    {
        private readonly IPostsService postsService;

        public HomePageController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [HttpGet]
        public IActionResult GetPosts(string searchTerm)
        {
            var posts = this.postsService.GetAllPosts<PostViewModel>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = posts
                    .Where(x => x.Title.ToLower().StartsWith(searchTerm, true, CultureInfo.InvariantCulture))
                    .ToList();
            }

            return this.View(posts);
        }
    }
}
