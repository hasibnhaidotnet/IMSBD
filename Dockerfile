# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj copy করে restore
COPY *.csproj ./
RUN dotnet restore

# বাকি সবকিছু copy করে publish
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Render PORT environment variable ব্যবহার করা হবে
CMD ["sh", "-c", "dotnet Inventorymanagementsystem.dll --urls http://0.0.0.0:$PORT"]
