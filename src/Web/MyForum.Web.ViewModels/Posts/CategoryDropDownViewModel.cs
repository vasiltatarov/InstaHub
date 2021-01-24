namespace MyForum.Web.ViewModels.Posts
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
