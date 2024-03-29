#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

EXPOSE 45665
EXPOSE 8891

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Galaxy/Galaxy.API/Galaxy.API.csproj", "Services/Galaxy/Galaxy.API/"]
COPY ["BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["BuildingBlocks/EventBus/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/EventBus/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
COPY ["Infrastructure/Infrastructure/Infrastructure.csproj", "Infrastructure/Infrastructure/"]
RUN dotnet restore "Services/Galaxy/Galaxy.API/Galaxy.API.csproj"
COPY . .
WORKDIR "/src/Services/Galaxy/Galaxy.API"
RUN dotnet build "Galaxy.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Galaxy.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Galaxy.API.dll"]