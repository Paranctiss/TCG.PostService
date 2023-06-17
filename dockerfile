#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Accept the PAT as an argument
ARG NUGET_PAT=PAT_VALUE_HERE

# Set the environment variable for NuGet
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/ProjetCSC/_packaging/ProjetCSC/nuget/v3/index.json\", \"username\":\"any\", \"password\":\"${NUGET_PAT}\"}]}"

COPY ["TCG.PostService.API/TCG.PostService.API.csproj", "TCG.PostService.API/"]
COPY ["TCG.PostService.Application/TCG.PostService.Application.csproj", "TCG.PostService.Application/"]
COPY ["TCG.PostService.Domain/TCG.PostService.Domain.csproj", "TCG.PostService.Domain/"]
COPY ["TCG.PostService.Persistence/TCG.PostService.Persistence.csproj", "TCG.PostService.Persistence/"]
COPY . .
WORKDIR "/src/TCG.PostService.API"
RUN dotnet restore
RUN dotnet build "TCG.PostService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TCG.PostService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TCG.PostService.API.dll"]
