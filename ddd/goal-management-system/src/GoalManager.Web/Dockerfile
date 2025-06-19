FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Create the directory before switching to non-root user
RUN mkdir -p /home/app/.aspnet/DataProtection-Keys && \
    chown -R ${APP_UID}:${APP_UID} /home/app/.aspnet

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GoalManager.Web/GoalManager.Web.csproj", "src/GoalManager.Web/"]
COPY ["src/GoalManager.Infrastructure/GoalManager.Infrastructure.csproj", "src/GoalManager.Infrastructure/"]
COPY ["src/GoalManager.Core/GoalManager.Core.csproj", "src/GoalManager.Core/"]
COPY ["src/GoalManager.UseCases/GoalManager.UseCases.csproj", "src/GoalManager.UseCases/"]
RUN dotnet restore "src/GoalManager.Web/GoalManager.Web.csproj"
COPY . .
WORKDIR "/src/src/GoalManager.Web"
RUN dotnet build "GoalManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GoalManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoalManager.Web.dll"]
