# استخدم ASP.NET Runtime كبيئة تشغيل
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# استخدم SDK للبناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./FifaAuthServer.csproj"
RUN dotnet publish "./FifaAuthServer.csproj" -c Release -o /app/publish

# نسخة التشغيل النهائية
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FifaAuthServer.dll"]
