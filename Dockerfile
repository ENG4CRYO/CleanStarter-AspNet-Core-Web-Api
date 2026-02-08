FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["CleanStarter.API/CleanStarter.API.csproj", "CleanStarter.API/"]
COPY ["CleanStarter.Application/CleanStarter.Application.csproj", "CleanStarter.Application/"]
COPY ["CleanStarter.Core/CleanStarter.Core.csproj", "CleanStarter.Core/"]
COPY ["CleanStarter.Infrastructure/CleanStarter.Infrastructure.csproj", "CleanStarter.Infrastructure/"]


RUN dotnet restore "CleanStarter.API/CleanStarter.API.csproj"

COPY . .
WORKDIR "/src/CleanStarter.API"
RUN dotnet build "CleanStarter.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanStarter.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


ENV ASPNETCORE_ENVIRONMENT=Development
ENV EnableScalar=true

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanStarter.API.dll"]