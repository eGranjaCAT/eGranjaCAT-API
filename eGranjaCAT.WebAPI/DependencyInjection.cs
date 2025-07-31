using eGranjaCAT.Application;

namespace eGranjaCAT.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI().AddInfrastructureDI();

            return services;
        }
    }
}