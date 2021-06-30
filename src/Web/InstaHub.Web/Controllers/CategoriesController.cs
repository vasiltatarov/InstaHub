namespace InstaHub.Web.Controllers
{
    using System;

    using InstaHub.Common;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Categories;
    using InstaHub.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    using static InstaHub.Common.GlobalConstants;

    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;

        public CategoriesController(ICategoryService categoryService, IPostService postService)
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }

        public IActionResult GetCategories() => this.View(
                new IndexViewModel()
                {
                    Categories = this.categoryService.GetAll<IndexCategoryViewModel>(),
                });

        public IActionResult ByName(string name, int page = DefaultPage)
        {
            var viewModel = this.categoryService.GetByName<CategoryViewModel>(name);
            viewModel.ForumPosts = this.postService
                .GetByCategoryId<PostInCategoryViewModel>(viewModel.Id, ItemsOnPaged, (page - DefaultPage) * ItemsOnPaged);

            var count = this.postService.GetCountByCategoryId(viewModel.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsOnPaged);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = DefaultPage;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }
    }
}
