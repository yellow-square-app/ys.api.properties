# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ys.api.properties/ys.api.properties.csproj ./
RUN dotnet restore "ys.api.properties.csproj"
COPY ys.api.properties/ ./
RUN dotnet publish "ys.api.properties.csproj" -c Release -o /app

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS serve
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000 5001

ENTRYPOINT ["dotnet", "ys.api.properties.dll"]
