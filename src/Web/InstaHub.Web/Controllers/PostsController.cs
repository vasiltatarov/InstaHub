namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserSavedPostsService userSavedPostsService;

        public PostsController(
            IPostService postService,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager,
            IUserSavedPostsService userSavedPostsService)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.userSavedPostsService = userSavedPostsService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoryService.GetAll<CategoryDropDownViewModel>();
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
            var postId = await this.postService.CreateAsync(input.Title, input.Content, input.CategoryId, user.Id);

            this.TempData["InfoMessage"] = "Forum post Created!";

            return this.RedirectToAction(nameof(this.ById), new { id = postId });
        }

        public async Task<IActionResult> ById(int id)
        {
            var postViewModel = await this.postService.GetById<PostViewModel>(id);

            var user = await this.userManager.GetUserAsync(this.User);
            this.TempData["IsPostSaved"] = await this.userSavedPostsService.IsPostSaved(user.Id, id);

            return this.View(postViewModel);
        }
    }
}
