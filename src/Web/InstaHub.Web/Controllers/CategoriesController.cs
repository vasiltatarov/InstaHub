namespace InstaHub.Web.Controllers
{
    using System;

    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Categories;
    using InstaHub.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public CategoriesController(ICategoriesService categoriesService, IPostsService postsService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public IActionResult GetCategories()
        {
            var indexViewModel = new IndexViewModel()
            {
                Categories = this.categoriesService.GetAll<IndexCategoryViewModel>(),
            };

            return this.View(indexViewModel);
        }

        public IActionResult ByName(string name, int page = 1)
        {
            var viewModel = this.categoriesService.GetByName<CategoryViewModel>(name);
            viewModel.ForumPosts = this.postsService
                .GetByCategoryId<PostInCategoryViewModel>(viewModel.Id, ItemsPerPage, (page - 1) * ItemsPerPage);

            var count = this.postsService.GetCountByCategoryId(viewModel.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }
    }
}
