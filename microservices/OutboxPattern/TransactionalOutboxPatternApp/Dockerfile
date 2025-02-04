#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TransactionalOutboxPatternApp/TransactionalOutboxPatternApp.csproj", "TransactionalOutboxPatternApp/"]
COPY ["MessageQueue/MessageQueue.csproj", "MessageQueue/"]
RUN dotnet restore "TransactionalOutboxPatternApp/TransactionalOutboxPatternApp.csproj"
COPY . .
WORKDIR "/src/TransactionalOutboxPatternApp"
RUN dotnet build "TransactionalOutboxPatternApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionalOutboxPatternApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionalOutboxPatternApp.dll"]