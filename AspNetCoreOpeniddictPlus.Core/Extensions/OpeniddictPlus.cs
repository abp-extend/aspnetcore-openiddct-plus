using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace AspNetCoreOpeniddictPlus.Core.Extensions;

public static class OpeniddictPlus
{
    public static IServiceCollection AddOpeniddictPlusDbContext<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        if (configuration is null) return services;
        var defaultConnectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(defaultConnectionString);
            options.UseOpenIddict();
        });

        return services;
    }

    public static IServiceCollection AddOpeniddictPlusServer<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var env = services.BuildServiceProvider().GetService<IWebHostEnvironment>();
        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<TDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token")
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetUserInfoEndpointUris("/connect/userinfo")
                    .SetDeviceAuthorizationEndpointUris("/connect/deviceauthorization")
                    .SetRevocationEndpointUris("/connect/revoke")
                    .SetJsonWebKeySetEndpointUris("/.well-known/jwks.json")
                    .SetEndSessionEndpointUris("/connect/endsession")
                    .SetEndUserVerificationEndpointUris("/connect/enduserverification");

                options.AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow()
                    .AllowHybridFlow()
                    .AllowRefreshTokenFlow()
                    .AllowDeviceAuthorizationFlow();

                options.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles,
                    "api");

                if (env != null && env.IsDevelopment())
                {
                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }


                options
                    .UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();
            });
        return services;
    }

    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }
    
    public static IServiceCollection AddUserService<TEntity, TDbContext>(this IServiceCollection services) 
        where TEntity : class
        where TDbContext : DbContext
    {
        services.AddScoped<IUserService<TEntity>, UserService<TEntity, TDbContext>>();
        return services;
    }
}