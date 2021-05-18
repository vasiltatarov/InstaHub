namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.Posts;

    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserSavedPostsService userSavedPostsService;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager,
            IUserSavedPostsService userSavedPostsService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.userSavedPostsService = userSavedPostsService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
            var viewModel = new PostCreateInputModel()
            {
                Categories = categories,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var postId = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, user.Id);

            this.TempData["InfoMessage"] = "Forum post Created!";

            return this.RedirectToAction(nameof(this.ById), new { id = postId });
        }

        public async Task<IActionResult> ById(int id)
        {
            var postViewModel = await this.postsService.GetById<PostViewModel>(id);

            var user = await this.userManager.GetUserAsync(this.User);
            this.TempData["IsPostSaved"] = await this.userSavedPostsService.IsPostSaved(user.Id, id);

            return this.View(postViewModel);
        }
    }
}
