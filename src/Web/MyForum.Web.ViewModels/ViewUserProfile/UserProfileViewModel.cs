namespace MyForum.Web.ViewModels.ViewUserProfile
{
    using System;

    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImagePath { get; set; }

        public UserProfilePostViewModel[] Posts { get; set; }
    }
}
