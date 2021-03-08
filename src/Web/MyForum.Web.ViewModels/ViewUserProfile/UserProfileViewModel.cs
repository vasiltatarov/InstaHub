namespace MyForum.Web.ViewModels.ViewUserProfile
{
    using System;
    using System.Collections.Generic;

    using MyForum.Data.Models;

    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImagePath { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
