namespace InstaHub.Services.Data.Tests.Models
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class CategoryModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
