# MicroservicesApp
🚀 Инструкция по запуску микросервисного приложения
1️⃣ Предустановки
Перед началом работы убедитесь, что у вас установлены:

Docker и Docker Compose
.NET SDK (если требуется локальная сборка)
EF Core CLI (если потребуется работа с миграциями):

dotnet tool install --global dotnet-ef

2️⃣ Создание общей сети
Микросервисы работают в сети microservices_network. Если она не создана, создайте её:

docker network create microservices_network

3️⃣ Запуск базы данных
База данных PostgreSQL разворачивается отдельно с помощью файла docker-compose.db.yml:

docker compose -f docker-compose.db.yml up -d

Проверяем, что контейнер запущен:

docker ps

Если postgres не подключился к сети, добавляем его вручную:

docker network connect microservices_network postgres

4️⃣ Применение миграций
После запуска БД обновляем схему базы данных, применяя миграции:

dotnet ef database update --project AuthService/AuthService.csproj

✅ Убедитесь, что в файле appsettings.json корректно указан ConnectionStrings__DefaultConnection.

5️⃣ Запуск микросервисов
Когда БД запущена, запускаем остальные сервисы:

docker compose up -d --build

👉 Флаг --build заставляет пересобирать образы перед запуском.

6️⃣ Проверка работы сервисов
📌 Проверить подключение к PostgreSQL из auth_service

docker exec -it auth_service getent hosts postgres

✅ Если вернётся IP-адрес postgres, значит, соединение установлено.

📌 Проверить таблицы в базе данных

docker exec -it postgres psql -U user -d authdb -c "\dt"

7️⃣ Остановка сервисов
Остановить микросервисы (без удаления данных):

docker compose down

Остановить только БД:

docker compose -f docker-compose.db.yml down

Удалить все данные (⚠️ включая базу!):

docker compose -f docker-compose.db.yml down -v

8️⃣ Обновление кода и контейнеров
Если изменился код, пересобираем сервисы:

docker compose up -d --build --force-recreate

Если изменилась БД, пересобираем её:

docker compose -f docker-compose.db.yml up -d --build --force-recreate