﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/RobotCleaner.Api/RobotCleaner.Api.csproj", "src/RobotCleaner.Api/"]
COPY ["src/RobotCleaner/RobotCleaner.csproj", "src/RobotCleaner/"]
RUN dotnet restore "src/RobotCleaner.Api/RobotCleaner.Api.csproj"
COPY . .
WORKDIR "/src/src/RobotCleaner.Api"
RUN dotnet build "RobotCleaner.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RobotCleaner.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RobotCleaner.Api.dll"]
