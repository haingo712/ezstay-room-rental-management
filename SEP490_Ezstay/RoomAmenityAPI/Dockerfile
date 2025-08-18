FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RoomAmenityAPI/RoomAmenityAPI.csproj", "RoomAmenityAPI/"]
RUN dotnet restore "RoomAmenityAPI/RoomAmenityAPI.csproj"
COPY . .
WORKDIR "/src/RoomAmenityAPI"
RUN dotnet build "./RoomAmenityAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RoomAmenityAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoomAmenityAPI.dll"]
