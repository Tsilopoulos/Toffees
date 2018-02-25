using System.Collections.Generic;
using System.Security.Claims;
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
                new IdentityResources.Profile()
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
                        new Secret("glucoseSecret".Sha256())
                    },
                    // include the following using claims in access token (in addition to subject id)
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Id
                    },
                    // this API defines two scopes
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "biometric_api.full_access",
                            DisplayName = "Full access to biometric API"
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
                // .NET Core API Gateway
                new Client
                {
                    ClientId = "apiGateway",
                    ClientName = ".NET Core API Gateway",
                    ClientUri = "http://localhost:52633",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("C309386B-ADFA-4A24-B693-2B211806204C".Sha256())
                    },
                    AllowedGrantTypes = new[] {GrantType.ResourceOwnerPassword},
                    AllowedCorsOrigins =
                    {
                        "http://localhost:52633"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                // SPA client using client credential flow
                new Client
                {
                    ClientId = "angular",
                    ClientName = "Angular SPA Client",
                    ClientUri = "http://localhost:4002",
                    AllowedGrantTypes = {
                        GrantType.Hybrid,
                        GrantType.ClientCredentials,
                    },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("93A905DE-7760-4E00-AFC9-B421820F6B70".Sha256())
                    },
                    RedirectUris =
                    {
                        "http://localhost:4002/glucose"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4002/home"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4002"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "biometric_api.full_access"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                
                // Postman API tool client
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman Windows Client dev tests",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
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
                }
            };
        }
    }
}