#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventService.API/EventService.API.csproj", "EventService.API/"]
COPY ["EventService.Domain/EventService.Domain.csproj", "EventService.Domain/"]
COPY ["EventService.Infrastructure/EventService.Infrastructure.csproj", "EventService.Infrastructure/"]
RUN dotnet restore "./EventService.API/EventService.API.csproj"
COPY . .
WORKDIR "/src/EventService.API"
RUN dotnet build "./EventService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventService.API.dll"]