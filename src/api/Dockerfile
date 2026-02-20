FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.csproj", "./"]
RUN dotnet restore "Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.csproj"
COPY . .
RUN dotnet build "Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.dll"]
