using AutoMapper;
using TicketingSystem.RazorWebsite.Mapping;

namespace TicketingSystem.Xunit.Tests.Factories
{
    public static class MapperTestFactory
    {
        public static IMapper GenerateMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}
