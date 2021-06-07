namespace InstaHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public interface ICategoriesService : IMapFrom<Category>
    {
        Task CreateAsync(string name, string title, string description, string imageUrl);

        IEnumerable<T> GetAll<T>(int? count = null);

        T GetByName<T>(string name);

        Task<T> GetByIdAsync<T>(int id);

        Task<bool> IsCategoryExists(int id);

        Task Update(int id, string name, string title, string description, string imageUrl, bool isDeleted, DateTime deletedOn, DateTime createdOn, DateTime modifiedOn);

        Task<bool> Delete(int id);
    }
}
