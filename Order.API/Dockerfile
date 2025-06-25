# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../OrderService.sln ./
COPY ../Order.API/Order.API.csproj Order.API/
COPY ../Order.Application/Order.Application.csproj Order.Application/
COPY ../Order.Domain/Order.Domain.csproj Order.Domain/
COPY ../Order.Infrastructure/Order.Infrastructure.csproj Order.Infrastructure/

RUN dotnet restore OrderService.sln
COPY ../ ./

WORKDIR /src/Order.API
RUN dotnet publish -c Release -o /app/publish

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# ✅ ADD THIS LINE
ENV ASPNETCORE_URLS=http://+:8090

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Order.API.dll"]
