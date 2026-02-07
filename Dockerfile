# استخدمنا صورة إصدار 8.0 (الأكثر استقراراً). غيرها لـ 9.0 أو 10.0 إذا توفرت
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. نسخ ملفات المشاريع
COPY ["CleanStarter.API/CleanStarter.API.csproj", "CleanStarter.API/"]
COPY ["CleanStarter.Application/CleanStarter.Application.csproj", "CleanStarter.Application/"]
COPY ["CleanStarter.Core/CleanStarter.Core.csproj", "CleanStarter.Core/"]
COPY ["CleanStarter.Infrastructure/CleanStarter.Infrastructure.csproj", "CleanStarter.Infrastructure/"]

# 2. استعادة المكتبات
RUN dotnet restore "CleanStarter.API/CleanStarter.API.csproj"

# 3. نسخ باقي الكود وبناء المشروع
COPY . .
WORKDIR "/src/CleanStarter.API"
RUN dotnet build "CleanStarter.API.csproj" -c Release -o /app/build

# 4. النشر (Publish)
FROM build AS publish
RUN dotnet publish "CleanStarter.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. التشغيل النهائي
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# المتغيرات البيئية لضمان عمل Scalar
ENV ASPNETCORE_ENVIRONMENT=Development
ENV EnableScalar=true

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanStarter.API.dll"]