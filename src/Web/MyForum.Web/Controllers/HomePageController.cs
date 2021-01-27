namespace MyForum.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.Posts;

    public class HomePageController : Controller
    {
        private readonly IPostsService postsService;

        public HomePageController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public IActionResult GetPosts()
        {
            var posts = this.postsService.GetAllPosts<PostViewModel>();

            return this.View(posts);
        }
    }
}
