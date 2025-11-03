# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore
COPY ["BogusExample.csproj", "./"]
RUN dotnet restore "BogusExample.csproj"

# Copy everything and build
COPY . .
RUN dotnet build "BogusExample.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BogusExample.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV PORT=80
ENV ASPNETCORE_URLS="http://+:80"

# Run the executable
ENTRYPOINT ["dotnet","./BogusExample.dll"]