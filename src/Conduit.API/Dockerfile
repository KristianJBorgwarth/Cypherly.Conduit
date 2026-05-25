FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Conduit.API/Conduit.API.csproj", "src/Conduit.API/"]
COPY ["src/Conduit.Domain/Conduit.Domain.csproj", "src/Conduit.Domain/"]
COPY ["src/Conduit.Application/Conduit.Application.csproj", "src/Conduit.Application/"]
COPY ["src/Conduit.Infrastructure/Conduit.Infrastructure.csproj", "src/Conduit.Infrastructure/"]
RUN dotnet restore "src/Conduit.API/Conduit.API.csproj"
COPY . .
WORKDIR "/src/src/Conduit.API"
RUN dotnet build "./Conduit.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Conduit.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conduit.API.dll"]
