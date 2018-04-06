FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY . .
COPY IdentityServer4Auth.cer IdentityServer4Auth.cer
COPY IdentityServer4Auth.pfx IdentityServer4Auth.pfx
COPY IdentityServer4Auth.pvk IdentityServer4Auth.pvk
RUN dotnet publish --output /output --configuration Release
FROM microsoft/aspnetcore:2.0.0
COPY --from=build /output /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "Toffees.Identity.dll" ]