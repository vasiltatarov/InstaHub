namespace InstaHub.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class UserMessages
    {
        public string UserName { get; set; }

        public string UserImagePath { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
