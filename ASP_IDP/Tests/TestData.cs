using ASP_IDP.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ASP_IDP.Tests
{
    public class TestData : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public TestData(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            await CreateApplicationsAsync();
            await CreateScopesAsync();
            await CreateRoles();

            async Task CreateApplicationsAsync()
            {
                var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

                //Hier wird der Client als Application in der Datenbank angelegt mit seiner Addresse
                //Die ClientID und die ClientSecret GUID muss dem Client zur Verfügung gestellt werden!
                //Es ist wichtig die Callback Urls zu authorisieren über RedirectUris und PostLogoutRedirecturis
                //Bei den Berechtigungen ist es wichtig die Scopes zu den ResourceServern mit anzugeben, da sonst 
                //kein Zugriff auf die ResourceServer erfolgen kann (403 Forbidden)
                if (await manager.FindByClientIdAsync("AngularMVC") == null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "AngularMVC",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Angular MVC client application",
                        PostLogoutRedirectUris =
                        {
                            new Uri("http://localhost:4200"),
                            new Uri("https://localhost:4200")
                        },
                        RedirectUris =
                        {
                            new Uri("http://localhost:4200"),
                            new Uri("https://localhost:4200")
                        },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.Logout,
                            Permissions.Endpoints.Token,
                            Permissions.Endpoints.Revocation,
                            Permissions.GrantTypes.ClientCredentials,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.RefreshToken,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Address,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                            Permissions.Prefixes.Scope + "api1",
                            Permissions.Prefixes.Scope + "api2"
                        }
                    });
                }

                //resource_server_1 & 2 & gateway werden hier als Application angelegt mit Introspection Berechtigung
                //damit diese bei der Access_Token Validierung über den Identity Server überprüfen können 
                //ob der Token stimmt oder nicht
                if (await manager.FindByClientIdAsync("resource_server_1") == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "resource_server_1",
                        ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }

                if (await manager.FindByClientIdAsync("resource_server_2") == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "resource_server_2",
                        ClientSecret = "70104051-0AB5-405A-87CB-3E0D7707C82E",
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }

                if (await manager.FindByClientIdAsync("gateway_server") == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "gateway_server",
                        ClientSecret = "AD4076B7-9AD0-4306-A091-5D915997B22E",
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }
            }

            async Task CreateScopesAsync()
            {
                var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

                //Hier müssen die ResourceServer als Scopes angelegt werden damit diese in den Access_Token geschrieben werden wenn der Client über den Scope besitzt.
                //ResourceServer1 bekommt den Namen "api1" und ResourceServer2 den Namen "api2". Diese müssen bei der Application mit angegeben werden als Scope Permission!
                if (await manager.FindByNameAsync("api1") == null)
                {
                    var descriptor = new OpenIddictScopeDescriptor
                    {
                        Name = "api1",
                        Resources =
                        {
                            "resource_server_1"
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }

                if (await manager.FindByNameAsync("api2") == null)
                {
                    var descriptor = new OpenIddictScopeDescriptor
                    {
                        Name = "api2",
                        Resources =
                        {
                            "resource_server_2"
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }
            }

            async Task CreateRoles()
            {
                var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (await RoleManager.FindByNameAsync("admin") == null)
                {
                    IdentityRole role = new IdentityRole("admin");

                    await RoleManager.CreateAsync(role);
                }

                if (await RoleManager.FindByNameAsync("normal") == null)
                {
                    IdentityRole role = new IdentityRole("normal");

                    await RoleManager.CreateAsync(role);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
