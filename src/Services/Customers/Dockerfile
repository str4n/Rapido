FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Customers/Rapido.Services.Customers.Api/Rapido.Services.Customers.Api.csproj", "src/Services/Customers/Rapido.Services.Customers.Api/"]
COPY ["src/Services/Customers/Rapido.Services.Customers.Core/Rapido.Services.Customers.Core.csproj", "src/Services/Customers/Rapido.Services.Customers.Core/"]
COPY ["src/Framework/Rapido.Framework.Common/Rapido.Framework.Common.csproj", "src/Framework/Rapido.Framework.Common/"]
COPY ["src/Framework/Rapido.Framework/Rapido.Framework.csproj", "src/Framework/Rapido.Framework/"]
COPY ["src/Framework/Rapido.Framework.Api/Rapido.Framework.Api.csproj", "src/Framework/Rapido.Framework.Api/"]
COPY ["src/Framework/Rapido.Framework.Auth/Rapido.Framework.Auth.csproj", "src/Framework/Rapido.Framework.Auth/"]
COPY ["src/Framework/Rapido.Framework.Contexts/Rapido.Framework.Contexts.csproj", "src/Framework/Rapido.Framework.Contexts/"]
COPY ["src/Framework/Rapido.Framework.CQRS/Rapido.Framework.CQRS.csproj", "src/Framework/Rapido.Framework.CQRS/"]
RUN dotnet restore "src/Services/Customers/Rapido.Services.Customers.Api/Rapido.Services.Customers.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Customers/Rapido.Services.Customers.Api"
RUN dotnet build "Rapido.Services.Customers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Rapido.Services.Customers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rapido.Services.Customers.Api.dll"]
