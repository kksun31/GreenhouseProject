#Сборка
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["GreenhouseApi.csproj", "./"]
RUN dotnet restore "GreenhouseApi.csproj"
COPY . .
RUN dotnet publish "GreenhouseApi.csproj" -c Release -o /app/publish

# Выполнение 
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
# Устанавливаем утилиту curl
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GreenhouseApi.dll"]