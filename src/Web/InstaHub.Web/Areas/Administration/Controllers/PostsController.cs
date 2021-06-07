namespace InstaHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Posts;

    public class PostsController : AdministrationController
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;

        public PostsController(IPostsService postsService, ICategoriesService categoriesService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postsService.GetById<EditPostViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
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

            await this.postsService
                .Edit(id, post.Title, post.Content, post.CategoryId, post.IsDeleted, post.DeletedOn, post.CreatedOn, post.ModifiedOn);

            return this.RedirectToAction("ById", "Posts", new { area = string.Empty, id });
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
