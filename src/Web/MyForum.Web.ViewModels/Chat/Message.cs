namespace MyForum.Web.ViewModels.Chat
{
    using System;

    public class Message
    {
        public string UserImagePath { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
