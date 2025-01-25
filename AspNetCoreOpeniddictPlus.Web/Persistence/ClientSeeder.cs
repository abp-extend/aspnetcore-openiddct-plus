using OpenIddict.Abstractions;

namespace AspNetCoreOpeniddictPlus.Web.Persistence;

public class ClientSeeder(IServiceProvider serviceProvider, ILogger<ClientSeeder> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<OpeniddictPlusDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await manager.FindByClientIdAsync("service-worker", cancellationToken) is null)
        {
            logger.LogInformation("Creating a new application 'service-worker'...");
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = "service-worker",
                    ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "cc",
                    },
                },
                cancellationToken
            );
        }

        if (await manager.FindByClientIdAsync("web-ui", cancellationToken) is null)
        {
            logger.LogInformation("Creating a new application 'web-ui'...");
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = "web-ui",
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Web UI Client",
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:4200"),
                        new Uri("https://localhost:3000"),
                        new Uri("https://localhost:7057"),
                    },
                    RedirectUris =
                    {
                        new Uri("https://localhost:4200"),
                        new Uri("https://localhost:3000"),
                        new Uri("https://oauth.pstmn.io/v1/callback"),
                        new Uri("https://localhost:7057/auth/callback"),
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                    },
                    Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                    },
                },
                cancellationToken
            );
        }

        
        if (await manager.FindByClientIdAsync("nextjs-client", cancellationToken) is null)
        {
            logger.LogInformation("Creating a new application 'nextjs-client'...");
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "nextjs-client",
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Openiddict Plus NextJs UI Client",
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:3000"),
                },
                RedirectUris =
                {
                    new Uri("https://localhost:3000/auth/oidc"),
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                },
                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                }
                
            }, cancellationToken);
        }
        
        if (await manager.FindByClientIdAsync("postman-client", cancellationToken) is null)
        {
            logger.LogInformation("Creating a new application 'postman-client'...");
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman-client",
                ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C208",
                DisplayName = "Postman Client",
               
                RedirectUris =
                {
                    new Uri("https://oauth.pstmn.io/v1/browser-callback"),
                    new Uri("https://oauth.pstmn.io/v1/callback")
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Scopes.Profile
                },
                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                }
                
            }, cancellationToken);
        }
        
        if (await scopeManager.FindByNameAsync("api", cancellationToken) is null)
        {
            logger.LogInformation("Creating a new scope 'api'...");
            await scopeManager.CreateAsync(
                new OpenIddictScopeDescriptor { Name = "api", Resources = { "resource-server" } },
                cancellationToken
            );
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
