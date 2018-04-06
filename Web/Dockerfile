FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish --output /output --configuration Release
FROM microsoft/aspnetcore:2.0.0
COPY --from=build /output /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "Toffees.Web.dll" ]