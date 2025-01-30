#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SearchWorkerService/SearchWorkerService.csproj", "SearchWorkerService/"]
RUN dotnet restore "SearchWorkerService/SearchWorkerService.csproj"
COPY . .
WORKDIR "/src/SearchWorkerService"
RUN dotnet build "SearchWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SearchWorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchWorkerService.dll"]