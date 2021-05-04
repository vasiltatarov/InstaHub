namespace MyForum.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<ChatMessage> chatMessageRepository;

        public ChatService(IDeletableEntityRepository<ChatMessage> chatMessageRepository)
        {
            this.chatMessageRepository = chatMessageRepository;
        }

        public async Task CreateAsync(string message, string userId)
        {
            var chatMessage = new ChatMessage
            {
                Message = message,
                UserId = userId,
            };

            await this.chatMessageRepository.AddAsync(chatMessage);
            await this.chatMessageRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetMessages<T>()
            => this.chatMessageRepository
                .All()
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToList();
    }
}
