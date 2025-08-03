using eGranjaCAT.Application.Mappings;
using System.Reflection;


namespace eGranjaCAT.Api.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(FarmProfile)),
                Assembly.GetAssembly(typeof(UserProfile)),
                Assembly.GetAssembly(typeof(EntradaProfile))
            );

            return services;
        }
    }
}