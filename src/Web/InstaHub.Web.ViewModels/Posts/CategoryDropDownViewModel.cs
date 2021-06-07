namespace InstaHub.Web.ViewModels.Posts
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
