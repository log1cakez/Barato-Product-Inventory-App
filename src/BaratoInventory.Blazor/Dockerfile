FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for dependencies
COPY ["src/BaratoInventory.Core/BaratoInventory.Core.csproj", "src/BaratoInventory.Core/"]
COPY ["src/BaratoInventory.Infrastructure/BaratoInventory.Infrastructure.csproj", "src/BaratoInventory.Infrastructure/"]
COPY ["src/BaratoInventory.Blazor/BaratoInventory.Blazor.csproj", "src/BaratoInventory.Blazor/"]

# Restore dependencies for Blazor project
RUN dotnet restore "src/BaratoInventory.Blazor/BaratoInventory.Blazor.csproj"

# Copy the rest of the source code
COPY . .

# Build the Blazor project
WORKDIR "/src/src/BaratoInventory.Blazor"
RUN dotnet build "BaratoInventory.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaratoInventory.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaratoInventory.Blazor.dll"]
