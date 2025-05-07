FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ControleDeLancamentos/ControleDeLancamentos.Api.csproj", "ControleDeLancamentos/"]
COPY ["ControleDeLancamentos.Application/ControleDeLancamentos.Application.csproj", "ControleDeLancamentos.Application/"]
COPY ["ControleDeLancamentos.Domain/ControleDeLancamentos.Domain.csproj", "ControleDeLancamentos.Domain/"]
COPY ["ControleDeLancamentos.Infrastructure/ControleDeLancamentos.Infrastructure.csproj", "ControleDeLancamentos.Infrastructure/"]
RUN dotnet restore "ControleDeLancamentos/ControleDeLancamentos.Api.csproj"
COPY . .
WORKDIR "/src/ControleDeLancamentos"
RUN dotnet build "ControleDeLancamentos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ControleDeLancamentos.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControleDeLancamentos.Api.dll"]