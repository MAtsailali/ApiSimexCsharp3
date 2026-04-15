# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos de proyecto y restaura
COPY ["ApiSimexCsharp3.csproj", "./"]
RUN dotnet restore "ApiSimexCsharp3.csproj"

# Copia el resto y publica
COPY . .
RUN dotnet publish "ApiSimexCsharp3.csproj" -c Release -o /app/publish

# Etapa final (ejecución)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish

# Obligamos a .NET a usar tu puerto específico
ENV ASPNETCORE_URLS=http://+:5198
EXPOSE 5198

ENTRYPOINT ["dotnet", "ApiSimexCsharp3.dll"]
