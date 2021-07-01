namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;

    public class ContactService : IContactService
    {
        private readonly IRepository<ContactForm> contactRepository;

        public ContactService(IRepository<ContactForm> contactRepository) => this.contactRepository = contactRepository;

        public async Task Add(string name, string email, string content, string ip)
        {
            var contactForm = new ContactForm
            {
                Name = name,
                Email = email,
                Content = content,
                Ip = ip,
            };

            await this.contactRepository.AddAsync(contactForm);
            await this.contactRepository.SaveChangesAsync();
        }
    }
}
