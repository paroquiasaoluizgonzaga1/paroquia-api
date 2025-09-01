# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy only project files first for better layer caching
COPY ["API/ParoquiaSLG.API/ParoquiaSLG.API.csproj", "API/ParoquiaSLG.API/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Modules/Notification/Modules.Notification.Application/Modules.Notification.Application.csproj", "Modules/Notification/Modules.Notification.Application/"]
COPY ["Modules/Notification/Modules.Notification.Infrastructure/Modules.Notification.Infrastructure.csproj", "Modules/Notification/Modules.Notification.Infrastructure/"]
COPY ["Modules/ParishManagement/Modules.ParishManagement.Application/Modules.ParishManagement.Application.csproj", "Modules/ParishManagement/Modules.ParishManagement.Application/"]
COPY ["Modules/ParishManagement/Modules.ParishManagement.Domain/Modules.ParishManagement.Domain.csproj", "Modules/ParishManagement/Modules.ParishManagement.Domain/"]
COPY ["Modules/ParishManagement/Modules.ParishManagement.Infrastructure/Modules.ParishManagement.Infrastructure.csproj", "Modules/ParishManagement/Modules.ParishManagement.Infrastructure/"]
COPY ["Modules/ParishManagement/Modules.ParishManagement.IntegrationEvents/Modules.ParishManagement.IntegrationEvents.csproj", "Modules/ParishManagement/Modules.ParishManagement.IntegrationEvents/"]
COPY ["Modules/ParishManagement/Modules.ParishManagement.Persistence/Modules.ParishManagement.Persistence.csproj", "Modules/ParishManagement/Modules.ParishManagement.Persistence/"]
COPY ["Modules/IdentityProvider/Modules.IdentityProvider.Application/Modules.IdentityProvider.Application.csproj", "Modules/IdentityProvider/Modules.IdentityProvider.Application/"]
COPY ["Modules/IdentityProvider/Modules.IdentityProvider.Domain/Modules.IdentityProvider.Domain.csproj", "Modules/IdentityProvider/Modules.IdentityProvider.Domain/"]
COPY ["Modules/IdentityProvider/Modules.IdentityProvider.Endpoints/Modules.IdentityProvider.Endpoints.csproj", "Modules/IdentityProvider/Modules.IdentityProvider.Endpoints/"]
COPY ["Modules/IdentityProvider/Modules.IdentityProvider.Infrastructure/Modules.IdentityProvider.Infrastructure.csproj", "Modules/IdentityProvider/Modules.IdentityProvider.Infrastructure/"]
COPY ["Modules/IdentityProvider/Modules.IdentityProvider.IntegrationEvents/Modules.IdentityProvider.IntegrationEvents.csproj", "Modules/IdentityProvider/Modules.IdentityProvider.IntegrationEvents/"]
COPY ["Modules/IdentityProvider/Modules.IdentityProvider.Persistence/Modules.IdentityProvider.Persistence.csproj", "Modules/IdentityProvider/Modules.IdentityProvider.Persistence/"]

# Restore dependencies
RUN dotnet restore "API/ParoquiaSLG.API/ParoquiaSLG.API.csproj"

# Copy source code
COPY . .
WORKDIR "/src/API/ParoquiaSLG.API"

# Publish stage
FROM build AS publish
RUN dotnet publish "ParoquiaSLG.API.csproj" -c Release -o /app/publish --no-restore

# Runtime stage - Using Alpine for smaller image size
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final

# Install necessary packages for Alpine
RUN apk add --no-cache icu-libs

# Set environment variables
ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Create non-root user for security
RUN addgroup -g 1001 -S appgroup && \
    adduser -u 1001 -S appuser -G appgroup

# Change ownership of the app directory
RUN chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

ENTRYPOINT ["dotnet", "ParoquiaSLG.API.dll"]