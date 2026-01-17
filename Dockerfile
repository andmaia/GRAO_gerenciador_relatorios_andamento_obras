# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj para restaurar
COPY src/GRAO.WEB/GRAO.WEB.csproj src/GRAO.WEB/
COPY src/GRAO.Core/GRAO.Core.csproj src/GRAO.Core/

# Restaurar o projeto WEB (restaura o Core como dependência)
RUN dotnet restore src/GRAO.WEB/GRAO.WEB.csproj

# Copiar código-fonte
COPY src src/

# Publicar WEB
WORKDIR /app/src/GRAO.WEB
RUN dotnet publish -c Release -o out \
    /p:UseAppHost=false

# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/src/GRAO.WEB/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "GRAO.WEB.dll"]
