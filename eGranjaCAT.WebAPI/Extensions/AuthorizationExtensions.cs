namespace eGranjaCAT.Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Entrades", policy =>
                    policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.HasClaim("Access", "Entrades")));

                options.AddPolicy("Lots", policy =>
                    policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.HasClaim("Access", "Lots")));

                options.AddPolicy("Farms", policy =>
                    policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.HasClaim("Access", "Farms")));
            });

            return services;
        }
    }
}
