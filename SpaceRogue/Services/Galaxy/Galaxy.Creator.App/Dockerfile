#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Galaxy/Galaxy.Creator.App/Galaxy.Creator.App.csproj", "Services/Galaxy/Galaxy.Creator.App/"]
COPY ["BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["BuildingBlocks/EventBus/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/EventBus/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
COPY ["Infrastructure/Infrastructure/Infrastructure.csproj", "Infrastructure/Infrastructure/"]
RUN dotnet restore "Services/Galaxy/Galaxy.Creator.App/Galaxy.Creator.App.csproj"
COPY . .
WORKDIR "/src/Services/Galaxy/Galaxy.Creator.App"
RUN dotnet build "Galaxy.Creator.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Galaxy.Creator.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Galaxy.Creator.App.dll"]