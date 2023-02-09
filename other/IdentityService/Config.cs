using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                // the api requires the role claim
                new ApiResource("myapi", "The Web Api", new[] { JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "myapi" }
                }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
             new ApiScope("myapi", "Access the api")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Duende.IdentityServer.Models.Client
            {
                ClientId = "clientapp",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedCorsOrigins = { "https://localhost:5021" },
                AllowedScopes = { "openid", "profile", "email", "myapi" },
                RedirectUris = { "https://localhost:5021/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:5021/" },
                Enabled = true
            }
        };
}

