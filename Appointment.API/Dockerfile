# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ../Appointment.API/Appointment.API.csproj ./Appointment.API/
COPY ../Appointment.Core/Appointment.Core.csproj ./Appointment.Core/
COPY ../Appointment.Domain/Appointment.Domain.csproj ./Appointment.Domain/
COPY ../Appointment.Infrastructure/Appointment.Infrastructure.csproj ./Appointment.Infrastructure/

# Restore dependencies
RUN dotnet restore "Appointment.API/Appointment.API.csproj"

# Copy the entire source code
COPY . .

# Build the application
WORKDIR /src/Appointment.API
RUN dotnet build "Appointment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Appointment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointment.API.dll"]
