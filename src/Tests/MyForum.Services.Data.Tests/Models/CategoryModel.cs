namespace MyForum.Services.Data.Tests.Models
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class CategoryModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
