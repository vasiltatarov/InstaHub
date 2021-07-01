namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    public interface IContactService
    {
        Task Add(string name, string email, string content, string ip);
    }
}
