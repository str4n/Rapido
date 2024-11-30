FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5100

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Docker
WORKDIR /src
COPY ["src/Saga/Rapido.Saga/Rapido.Saga.csproj", "src/Saga/Rapido.Saga/"]
COPY ["src/Framework/Rapido.Framework/Rapido.Framework.csproj", "src/Framework/Rapido.Framework/"]
COPY ["src/Framework/Rapido.Framework.Api/Rapido.Framework.Api.csproj", "src/Framework/Rapido.Framework.Api/"]
COPY ["src/Framework/Rapido.Framework.Common/Rapido.Framework.Common.csproj", "src/Framework/Rapido.Framework.Common/"]
COPY ["src/Framework/Rapido.Framework.Auth/Rapido.Framework.Auth.csproj", "src/Framework/Rapido.Framework.Auth/"]
COPY ["src/Framework/Rapido.Framework.Contexts/Rapido.Framework.Contexts.csproj", "src/Framework/Rapido.Framework.Contexts/"]
COPY ["src/Framework/Rapido.Framework.HTTP/Rapido.Framework.HTTP.csproj", "src/Framework/Rapido.Framework.HTTP/"]
COPY ["src/Framework/Rapido.Framework.Messaging.RabbitMQ/Rapido.Framework.Messaging.RabbitMQ.csproj", "src/Framework/Rapido.Framework.Messaging.RabbitMQ/"]
COPY ["src/Framework/Rapido.Framework.Messaging/Rapido.Framework.Messaging.csproj", "src/Framework/Rapido.Framework.Messaging/"]
COPY ["src/Framework/Rapido.Framework.Observability/Rapido.Framework.Observability.csproj", "src/Framework/Rapido.Framework.Observability/"]
COPY ["src/Framework/Rapido.Framework.Vault/Rapido.Framework.Vault.csproj", "src/Framework/Rapido.Framework.Vault/"]
RUN dotnet restore "src/Saga/Rapido.Saga/Rapido.Saga.csproj"
COPY . .
WORKDIR "/src/src/Saga/Rapido.Saga"
RUN dotnet build "Rapido.Saga.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Docker
RUN dotnet publish "Rapido.Saga.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rapido.Saga.dll"]
