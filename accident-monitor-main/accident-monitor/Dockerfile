# Base image for runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src-app

# Copy solution and project files for restore
COPY ["*.sln", "*.props", "src/**/*.csproj", "tests/**/*.csproj", "./"]
RUN dotnet restore "AccidentMonitor.sln"

# Copy entire source code and build
COPY . ./
WORKDIR /src-app/src/Web
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /accident-monitor-app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Install Node.js and npm during build only
RUN apt-get update && apt-get install -y nodejs npm && \
    dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /accident-monitor-app/publish /p:UseAppHost=false && \
    apt-get remove -y nodejs npm && apt-get clean

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /accident-monitor-app/publish .

# Set environment variables
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80

# Entry point for the application
ENTRYPOINT ["dotnet", "AccidentMonitor.Web.dll"]