#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AuditLogApi/AuditLogApi.csproj", "AuditLogApi/"]
RUN dotnet restore "AuditLogApi/AuditLogApi.csproj"
COPY . .
WORKDIR "/src/AuditLogApi"
RUN dotnet build "AuditLogApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuditLogApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuditLogApi.dll"]