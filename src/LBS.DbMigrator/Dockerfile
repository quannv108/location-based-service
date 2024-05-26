FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS base
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS publish
ARG BUILD_CONFIGURATION=Release
ARG RUNTIME=linux-musl-x64
WORKDIR /src
COPY ["src/LBS.DbMigrator/LBS.DbMigrator.csproj", "LBS.DbMigrator/"]
COPY ["src/LBS.Core/LBS.Core.csproj", "LBS.Core/"]
COPY ["src/LBS.Dal/LBS.Dal.csproj", "LBS.Dal/"]
COPY ["src/LBS.Dal.EF/LBS.Dal.EF.csproj", "LBS.Dal.EF/"]
COPY ["src/LBS.Data.Models/LBS.Data.Models.csproj", "LBS.Data.Models/"]
RUN dotnet restore "LBS.DbMigrator/LBS.DbMigrator.csproj" --runtime $RUNTIME
COPY src .
WORKDIR "/src/LBS.DbMigrator"
RUN dotnet publish "LBS.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:AnalyzeConfiguration=false --no-restore --runtime $RUNTIME

FROM base AS final
USER root
RUN apk add --no-cache  icu-data-full icu-libs \
    && rm -rf /var/cache/apk/* \
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LBS.DbMigrator.dll"]
