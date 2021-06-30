namespace InstaHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AdministrationController
    {
        private readonly IPostService postService;
        private readonly ICategoryService categoryService;

        public PostsController(IPostService postService, ICategoryService categoryService)
        {
            this.postService = postService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postService.GetById<EditPostViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var categories = this.categoryService.GetAll<CategoryDropDownViewModel>();
            post.Categories = categories;

            return this.View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPostViewModel post)
        {
            if (id != post.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(post);
            }

            await this.postService
                .Edit(id, post.Title, post.Content, post.CategoryId, post.IsDeleted, post.DeletedOn, post.CreatedOn, post.ModifiedOn);

            return this.RedirectToAction("ById", "Posts", new { area = string.Empty, id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.postService.Delete(id);

            if (!isDeleted)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Posts", "HomePage", new { area = string.Empty });
        }
    }
}
