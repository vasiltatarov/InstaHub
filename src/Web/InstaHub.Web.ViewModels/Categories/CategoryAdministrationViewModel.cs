namespace InstaHub.Web.ViewModels.Categories
{
    using System;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class CategoryAdministrationViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
