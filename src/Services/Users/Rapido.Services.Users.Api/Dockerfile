﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5020


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Docker
WORKDIR /src
COPY ["src/Services/Users/Rapido.Services.Users.Api/Rapido.Services.Users.Api.csproj", "src/Services/Users/Rapido.Services.Users.Api/"]
COPY ["src/Services/Users/Rapido.Services.Users.Core/Rapido.Services.Users.Core.csproj", "src/Services/Users/Rapido.Services.Users.Core/"]
COPY ["src/Framework/Rapido.Framework.Auth/Rapido.Framework.Auth.csproj", "src/Framework/Rapido.Framework.Auth/"]
COPY ["src/Framework/Rapido.Framework.Common/Rapido.Framework.Common.csproj", "src/Framework/Rapido.Framework.Common/"]
COPY ["src/Framework/Rapido.Framework.Contexts/Rapido.Framework.Contexts.csproj", "src/Framework/Rapido.Framework.Contexts/"]
COPY ["src/Framework/Rapido.Framework.Postgres/Rapido.Framework.Postgres.csproj", "src/Framework/Rapido.Framework.Postgres/"]
COPY ["src/Framework/Rapido.Framework/Rapido.Framework.csproj", "src/Framework/Rapido.Framework/"]
COPY ["src/Framework/Rapido.Framework.Api/Rapido.Framework.Api.csproj", "src/Framework/Rapido.Framework.Api/"]
COPY ["src/Framework/Rapido.Framework.HTTP/Rapido.Framework.HTTP.csproj", "src/Framework/Rapido.Framework.HTTP/"]
COPY ["src/Framework/Rapido.Framework.Messaging.RabbitMQ/Rapido.Framework.Messaging.RabbitMQ.csproj", "src/Framework/Rapido.Framework.Messaging.RabbitMQ/"]
COPY ["src/Framework/Rapido.Framework.Messaging/Rapido.Framework.Messaging.csproj", "src/Framework/Rapido.Framework.Messaging/"]
COPY ["src/Framework/Rapido.Framework.Observability/Rapido.Framework.Observability.csproj", "src/Framework/Rapido.Framework.Observability/"]
RUN dotnet restore "src/Services/Users/Rapido.Services.Users.Api/Rapido.Services.Users.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Users/Rapido.Services.Users.Api"
RUN dotnet build "Rapido.Services.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Docker
RUN dotnet publish "Rapido.Services.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rapido.Services.Users.Api.dll"]
