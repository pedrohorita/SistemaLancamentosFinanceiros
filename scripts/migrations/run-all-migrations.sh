#!/bin/bash
set -e

echo "Esperando SQL Server inicializar..."
sleep 30

echo "=== Diagnóstico: Listando estrutura de diretórios ==="
pwd
ls -la
echo "=== Arquivos .csproj encontrados ==="
find . -name "*.csproj" | sort
echo "=== Arquivos .sln encontrados ==="
find . -name "*.sln" | sort

echo "=== Restaurando dependências específicas ==="

dotnet restore ./ControleDeLancamentos/ControleDeLancamentos/ControleDeLancamentos.Api.csproj
dotnet restore ./ControleDeLancamentos/ControleDeLancamentos.Infrastructure/ControleDeLancamentos.Infrastructure.csproj
dotnet restore ./ConsolidadoDiario/ConsolidadoDiario/ConsolidadoDiario.Api.csproj
dotnet restore ./ConsolidadoDiario/ConsolidadoDiario.Infrastructure/ConsolidadoDiario.Infrastructure.csproj

echo "=== Aplicando migrações ControleDeLancamentos ==="

dotnet ef database update \
  --project ./ControleDeLancamentos/ControleDeLancamentos.Infrastructure/ControleDeLancamentos.Infrastructure.csproj \
  --startup-project ./ControleDeLancamentos/ControleDeLancamentos/ControleDeLancamentos.Api.csproj \
  --verbose

echo "=== Aplicando migrações ConsolidadoDbContext ==="
dotnet ef database update \
  --context ConsolidadoDbContext \
  --project ./ConsolidadoDiario/ConsolidadoDiario.Infrastructure/ConsolidadoDiario.Infrastructure.csproj \
  --startup-project ./ConsolidadoDiario/ConsolidadoDiario/ConsolidadoDiario.Api.csproj \
  --verbose

echo "=== Aplicando migrações ConsolidadoCategoriaDbContext ==="
dotnet ef database update \
  --context ConsolidadoCategoriaDbContext \
  --project ./ConsolidadoDiario/ConsolidadoDiario.Infrastructure/ConsolidadoDiario.Infrastructure.csproj \
  --startup-project ./ConsolidadoDiario/ConsolidadoDiario/ConsolidadoDiario.Api.csproj \
  --verbose

echo "=== Migrações concluídas com sucesso! ==="