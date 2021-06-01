namespace MyForum.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;

    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class EditPostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int VisitorsCount { get; set; }

        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
