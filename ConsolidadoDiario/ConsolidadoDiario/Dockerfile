FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConsolidadoDiario/ConsolidadoDiario.Api.csproj", "ConsolidadoDiario/"]
COPY ["ConsolidadoDiario.Application/ConsolidadoDiario.Application.csproj", "ConsolidadoDiario.Application/"]
COPY ["ConsolidadoDiario.Domain/ConsolidadoDiario.Domain.csproj", "ConsolidadoDiario.Domain/"]
COPY ["ConsolidadoDiario.Infrastructure/ConsolidadoDiario.Infrastructure.csproj", "ConsolidadoDiario.Infrastructure/"]
RUN dotnet restore "ConsolidadoDiario/ConsolidadoDiario.Api.csproj"
COPY . .
WORKDIR "/src/ConsolidadoDiario"
RUN dotnet build "ConsolidadoDiario.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsolidadoDiario.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsolidadoDiario.Api.dll"]