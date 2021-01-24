namespace MyForum.Web.ViewModels.Home
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class IndexCategoryViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int PostsCount { get; set; }

        public string Url => $"/p/{this.Name.Replace(' ', '-')}";
    }
}
