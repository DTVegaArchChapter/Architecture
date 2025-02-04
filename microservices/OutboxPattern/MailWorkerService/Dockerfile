#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MailWorkerService/MailWorkerService.csproj", "MailWorkerService/"]
RUN dotnet restore "MailWorkerService/MailWorkerService.csproj"
COPY . .
WORKDIR "/src/MailWorkerService"
RUN dotnet build "MailWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MailWorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailWorkerService.dll"]