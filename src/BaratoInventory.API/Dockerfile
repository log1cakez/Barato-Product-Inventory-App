FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for dependencies
COPY ["src/BaratoInventory.Core/BaratoInventory.Core.csproj", "src/BaratoInventory.Core/"]
COPY ["src/BaratoInventory.Infrastructure/BaratoInventory.Infrastructure.csproj", "src/BaratoInventory.Infrastructure/"]
COPY ["src/BaratoInventory.API/BaratoInventory.API.csproj", "src/BaratoInventory.API/"]

# Restore dependencies for API project
RUN dotnet restore "src/BaratoInventory.API/BaratoInventory.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the API project
WORKDIR "/src/src/BaratoInventory.API"
RUN dotnet build "BaratoInventory.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaratoInventory.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaratoInventory.API.dll"]
