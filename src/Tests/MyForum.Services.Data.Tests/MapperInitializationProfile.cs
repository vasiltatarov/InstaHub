namespace MyForum.Services.Data.Tests
{
    using System.Reflection;

    using AutoMapper;
    using MyForum.Services.Mapping;

    public class MapperInitializationProfile : Profile
    {
        public MapperInitializationProfile()
        {
            AutoMapperConfig.RegisterMappings(Assembly.GetCallingAssembly());
        }
    }
}
