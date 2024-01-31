# Set up base image to build the docker container and expose ports for our API
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80 
EXPOSE 443

# Copy project files into the container itself and build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src
COPY ["src/GoVilla.API/GoVilla.API.csproj", "src/GoVilla.API/"]
COPY ["src/GoVilla.Application/GoVilla.Application.csproj", "src/GoVilla.Application/"]
COPY ["src/GoVilla.Domain/GoVilla.Domain.csproj", "src/GoVilla.Domain/"]
COPY ["src/GoVilla.Infrastructure/GoVilla.Infrastructure.csproj", "src/GoVilla.Infrastructure/"]
RUN dotnet restore "src/GoVilla.API/GoVilla.API.csproj"
COPY . .
WORKDIR "/src/src/GoVilla.API"
RUN dotnet build "GoVilla.API.csproj" --configuration Release --no-restore --nologo --output /app/build 

# Publish the project
FROM build AS publish
RUN dotnet publish "GoVilla.API.csproj" --configuration Release --no-restore --nologo --output /app/publish

# Set up the final image, copy the published application from the publish image and specify the entry point for running the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoVilla.API.dll"]