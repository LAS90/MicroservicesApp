# 1. Базовый образ с ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# 2. SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 3. Копируем проект целиком и восстанавливаем зависимости
COPY ApiGateway/ ApiGateway/
WORKDIR /src/ApiGateway
RUN dotnet restore

# 4. Компилируем и публикуем
RUN dotnet publish -c Release -o /app/publish

# 5. Финальный образ
FROM base AS final
WORKDIR /app

# 6. Копируем опубликованные файлы
COPY --from=build /app/publish .

# 7. Запуск
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
