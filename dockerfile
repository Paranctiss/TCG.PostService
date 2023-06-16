#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR ../app
EXPOSE 7239

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR ../src
COPY ["TCG.PostService.API/TCG.PostService.API.csproj", "TCG.PostService.API/"]
COPY ["TCG.PostService.Application/TCG.PostService.Application.csproj", "TCG.PostService.Application/"]
COPY ["TCG.PostService.Domain/TCG.PostService.Domain.csproj", "TCG.PostService.Domain/"]
COPY ["TCG.PostService.Persistence/TCG.PostService.Persistence.csproj", "TCG.PostService.Persistence/"]
RUN dotnet restore "TCG.PostService.API/TCG.PostService.API.csproj"
COPY . .
WORKDIR "../src/TCG.PostService.API"
RUN dotnet build "TCG.PostService.API.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "TCG.PostService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TCG.PostService.API.dll"]