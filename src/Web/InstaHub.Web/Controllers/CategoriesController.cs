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
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public CategoriesController(ICategoriesService categoriesService, IPostsService postsService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public IActionResult GetCategories() => this.View(
                new IndexViewModel()
                {
                    Categories = this.categoriesService.GetAll<IndexCategoryViewModel>(),
                });

        public IActionResult ByName(string name, int page = DefaultPage)
        {
            var viewModel = this.categoriesService.GetByName<CategoryViewModel>(name);
            viewModel.ForumPosts = this.postsService
                .GetByCategoryId<PostInCategoryViewModel>(viewModel.Id, ItemsOnPaged, (page - DefaultPage) * ItemsOnPaged);

            var count = this.postsService.GetCountByCategoryId(viewModel.Id);
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
