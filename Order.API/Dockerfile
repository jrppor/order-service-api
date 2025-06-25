# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first
COPY ../OrderService.sln ./
COPY ../Order.API/Order.API.csproj Order.API/
COPY ../Order.Application/Order.Application.csproj Order.Application/
COPY ../Order.Domain/Order.Domain.csproj Order.Domain/
COPY ../Order.Infrastructure/Order.Infrastructure.csproj Order.Infrastructure/

# Restore
RUN dotnet restore OrderService.sln

# Copy everything else
COPY ../ ./

# Publish project
WORKDIR /src/Order.API
RUN dotnet publish -c Release -o /app/publish

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Order.API.dll"]
