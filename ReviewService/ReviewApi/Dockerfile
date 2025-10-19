# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /

# Copy all source code
COPY . .

# Restore dependencies
RUN dotnet restore "ReviewService/ReviewApi/ReviewApi.csproj"

# Build the application
RUN dotnet build "ReviewService/ReviewApi/ReviewApi.csproj" -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "ReviewService/ReviewApi/ReviewApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ReviewApi.dll"]
