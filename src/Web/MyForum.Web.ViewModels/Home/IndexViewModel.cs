namespace MyForum.Web.ViewModels.Home
{
    using System.Collections;
    using System.Collections.Generic;

    using MyForum.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<IndexCategoryViewModel> Categories { get; set; }
    }
}
