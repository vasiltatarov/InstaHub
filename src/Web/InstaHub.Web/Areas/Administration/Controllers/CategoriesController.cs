namespace InstaHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Area("Administration")]
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
            => this.categoryService = categoryService;

        public IActionResult Index()
            => this.View(this.categoryService.GetAll<CategoryAdministrationViewModel>());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categoryService.GetByIdAsync<CategoryAdministrationViewModel>(id.Value);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        public IActionResult Create()
            => this.View(new CreateCategoryInputModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoryService.CreateAsync(input.Name, input.Title, input.Description, input.ImageUrl);
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categoryService.GetByIdAsync<CategoryAdministrationViewModel>(id.Value);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryAdministrationViewModel category)
        {
            if (id != category.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.categoryService
                        .Update(id, category.Name, category.Title, category.Description, category.ImageUrl, category.IsDeleted, category.DeletedOn, category.CreatedOn, category.ModifiedOn);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.CategoryExists(category.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categoryService.GetByIdAsync<CategoryAdministrationViewModel>(id.Value);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await this.categoryService.Delete(id);

            if (!isDeleted)
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        private async Task<bool> CategoryExists(int id)
            => await this.categoryService.IsCategoryExists(id);
    }
}
