# 1. ������� ����� � ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# 2. SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 3. �������� ������ ������� � ��������������� �����������
COPY ApiGateway/ ApiGateway/
WORKDIR /src/ApiGateway
RUN dotnet restore

# 4. ����������� � ���������
RUN dotnet publish -c Release -o /app/publish

# 5. ��������� �����
FROM base AS final
WORKDIR /app

# 6. �������� �������������� �����
COPY --from=build /app/publish .

# 7. ������
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
