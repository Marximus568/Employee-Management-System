# ------------------------------
# Stage 1: Base runtime image
# ------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# ------------------------------
# Stage 2: Build
# ------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore
COPY ["Test_Express/Test_Express.csproj", "Test_Express/"]
RUN dotnet restore "Test_Express/Test_Express.csproj"

# Copy all source files
COPY . .
WORKDIR "/src/Test_Express"

# Build the project
RUN dotnet build "./Test_Express.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ------------------------------
# Stage 3: Publish
# ------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Test_Express.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ------------------------------
# Stage 4: Final image
# ------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Test_Express.dll"]
