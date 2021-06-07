namespace InstaHub.Services.Data.Tests
{
    using System.Reflection;

    using AutoMapper;
    using InstaHub.Services.Mapping;

    public class MapperInitializationProfile : Profile
    {
        public MapperInitializationProfile()
        {
            AutoMapperConfig.RegisterMappings(Assembly.GetCallingAssembly());
        }
    }
}
