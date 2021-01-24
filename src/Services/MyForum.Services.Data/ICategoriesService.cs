namespace MyForum.Services.Data
{
    using System.Collections.Generic;

    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public interface ICategoriesService : IMapFrom<Category>
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetByName<T>(string name);
    }
}
