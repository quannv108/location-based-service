FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["test/Lbs.App.HttpApi.StressTests/Lbs.App.HttpApi.StressTests.csproj", "test/Lbs.App.HttpApi.StressTests/"]
RUN dotnet restore "test/Lbs.App.HttpApi.StressTests/Lbs.App.HttpApi.StressTests.csproj"
COPY . .
WORKDIR "/src/test/Lbs.App.HttpApi.StressTests"
RUN dotnet build "Lbs.App.HttpApi.StressTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Lbs.App.HttpApi.StressTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lbs.App.HttpApi.StressTests.dll"]
