using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Toffees.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "biometric_api",
                    DisplayName = "Biometric API",
                    Description = "Biometric microservice API resource access",
                    Enabled = true,
                    // secret for using introspection endpoint
                    ApiSecrets =
                    {
                        new Secret("angularSecret".Sha256())
                    },
                    // include the following using claims in access token (in addition to subject id)
                    UserClaims =
                    {
                        JwtClaimTypes.Name
                    },
                    // this API defines two scopes
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "biometric_api.full_access",
                            DisplayName = "Full access to biometric API",
                        },
                        new Scope
                        {
                            Name = "biometric_api.read_only",
                            DisplayName = "Read only access to biometric API"
                        }
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "biometric_api.full_access" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "ASP.NET Core MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "biometric_api.full_access"
                    }
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "angular",
                    ClientName = "Angular SPA Client",
                    ClientUri = "http://localhost:5001",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("93A905DE-7760-4E00-AFC9-B421820F6B70".Sha256())
                    },
                    RedirectUris =
                    {
                        "http://localhost:5001/index.html",
                        "http://localhost:5001/callback.html",
                        "http://localhost:5001/silent.html",
                        "http://localhost:5001/popup.html",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5001/index.html" },
                    AllowedCorsOrigins = { "http://localhost:5001" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "biometric_api.full_access"
                    }
                },
                
                // Postman API tool client
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman Windows Client dev tests",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    Enabled = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "biometric_api.full_access"
                    },
                    AllowOfflineAccess = true
                },
            };
        }
    }
}