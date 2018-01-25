﻿using System.Collections.Generic;
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
                        new Secret("glucoseSecret".Sha256())
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
                // SPA client using client credential flow
                new Client
                {
                    ClientId = "angular",
                    ClientName = "Angular SPA Client",
                    ClientUri = "http://localhost:5002",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("93A905DE-7760-4E00-AFC9-B421820F6B70".Sha256())
                    },
                    RedirectUris =
                    {
                        "http://localhost:5002/home"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:5002"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:5002"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "biometric_api.full_access"
                    },
                    AllowOfflineAccess = true
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
                },
            };
        }
    }
}