FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UtilityRate.GrpcContracts/UtilityRate.GrpcContracts.csproj", "UtilityRate.GrpcContracts/"]
RUN dotnet restore "UtilityRate.GrpcContracts/UtilityRate.GrpcContracts.csproj"
COPY . .
WORKDIR "/src/UtilityRate.GrpcContracts"
RUN dotnet build "./UtilityRate.GrpcContracts.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UtilityRate.GrpcContracts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UtilityRate.GrpcContracts.dll"]
