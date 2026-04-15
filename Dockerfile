# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia el proyecto y restaura
COPY ["ApiSimexCsharp.csproj", "./"]
RUN dotnet restore "ApiSimexCsharp.csproj"

# 2. Copia el resto y publica
COPY . .
RUN dotnet publish "ApiSimexCsharp.csproj" -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# CORRECCIÓN AQUÍ: Añadido el punto al final
COPY --from=build /app/publish .

# Configuración de puerto
ENV ASPNETCORE_URLS=http://+:5198
EXPOSE 5198

ENTRYPOINT ["dotnet", "ApiSimexCsharp.dll"]