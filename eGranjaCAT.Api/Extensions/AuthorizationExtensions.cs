using eGranjaCAT.Domain.Enums;


namespace eGranjaCAT.Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var access in Enum.GetValues<AccessesEnum>())
                {
                    options.AddPolicy(access.ToString(), policy =>
                        policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.HasClaim("Access", access.ToString())));
                }
            });

            return services;
        }
    }
}