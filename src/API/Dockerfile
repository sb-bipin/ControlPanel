# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE ${PORT:-8080}

# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all source code
COPY . .

# Restore the API project
RUN dotnet restore "src/API/API.csproj"

# Build the API project
WORKDIR "/src/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage (THIS WAS MISSING)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENTRYPOINT ["dotnet", "API.dll"]