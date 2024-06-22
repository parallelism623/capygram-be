# Use base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ./capygram.Auth/capygram.Auth.csproj capygram.Auth/
COPY ./capygram.Common/capygram.Common.csproj capygram.Common/
RUN dotnet restore "capygram.Auth/capygram.Auth.csproj"

# Copy the rest of the files and build the project
COPY ./capygram.Auth/ capygram.Auth/
COPY ./capygram.Common/ capygram.Common/
WORKDIR "/src/capygram.Auth"
RUN dotnet build "capygram.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "capygram.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "capygram.Auth.dll"]
