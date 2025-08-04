FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Conduit.API/Conduit.API.csproj", "Conduit.API/"]
COPY ["Conduit.Domain/Conduit.Domain.csproj", "Conduit.Domain/"]
COPY ["Conduit.Application/Conduit.Application.csproj", "Conduit.Application/"]
COPY ["Conduit.Infrastructure/Conduit.Infrastructure.csproj", "Conduit.Infrastructure/"]
RUN dotnet restore "Conduit.API/Conduit.API.csproj"
COPY . .
WORKDIR "/src/Conduit.API"
RUN dotnet build "./Conduit.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Conduit.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conduit.API.dll"]
