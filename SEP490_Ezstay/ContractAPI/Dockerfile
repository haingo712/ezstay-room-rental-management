FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContractAPI/ContractAPI.csproj", "ContractAPI/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "ContractAPI/ContractAPI.csproj"
COPY . .
WORKDIR "/src/ContractAPI"
RUN dotnet build "ContractAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContractAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractAPI.dll"]
