# 1. Базовый образ с ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

# Явно включаем HTTP/2 в Kestrel
ENV DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2SUPPORT=true

# 2. SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 3. Сначала копируем весь проект
COPY AuthService/ AuthService/
WORKDIR /src/AuthService

# 4. Восстанавливаем зависимости
RUN dotnet restore

# 5. Собираем проект
RUN dotnet publish -c Release -o /app/publish

# 6. Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
