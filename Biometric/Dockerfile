FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5001

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Biometric/Toffees.Glucose.csproj Biometric/
RUN dotnet restore
COPY . .
WORKDIR /src/Biometric
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Toffees.Glucose.dll"]
