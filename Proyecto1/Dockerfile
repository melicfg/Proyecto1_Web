# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the working directory
COPY . .

# Build the project
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/out .

# Expose port 80 to the outside world
EXPOSE 80

# Define the command to run your app
CMD ["dotnet", "Proyecto1.dll"]
