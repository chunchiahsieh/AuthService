
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Api.Authorization;

namespace Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthorization(
               /* options =>
                {
                    options.AddPolicy("EditUserPolicy", policy =>
                        policy.Requirements.Add(new PermissionRequirement("EditUser")));
                }*/
            );

            return services;
        }
    }
}
