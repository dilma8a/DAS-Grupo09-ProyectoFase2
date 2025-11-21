# Script para iniciar el sistema completo
# Ejecutar como: .\IniciarSistema.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  EnvíoExpress SV - Sistema de Envíos  " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "DAS-Grupo09-ProyectoFase2.csproj")) {
    Write-Host "Error: Este script debe ejecutarse desde la carpeta raíz del proyecto" -ForegroundColor Red
    exit 1
}

Write-Host "1. Iniciando API REST..." -ForegroundColor Yellow
Write-Host ""

# Iniciar el API REST en una nueva ventana de PowerShell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'DAS-Grupo09-ProyectoFase2Rest'; Write-Host 'API REST - Puerto 7218' -ForegroundColor Green; dotnet run"

Write-Host "   API REST iniciándose en https://localhost:7218" -ForegroundColor Green
Write-Host ""

# Esperar 5 segundos para que el API REST inicie
Write-Host "2. Esperando 5 segundos para que el API REST inicie..." -ForegroundColor Yellow
Start-Sleep -Seconds 5
Write-Host ""

Write-Host "3. Iniciando aplicación MVC..." -ForegroundColor Yellow
Write-Host ""

# Iniciar la aplicación MVC en una nueva ventana de PowerShell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Write-Host 'Aplicación MVC - Puerto 7276 (HTTPS) / 5288 (HTTP)' -ForegroundColor Green; dotnet run"

Write-Host "   Aplicación MVC iniciándose en:" -ForegroundColor Green
Write-Host "   - HTTPS: https://localhost:7276" -ForegroundColor Green
Write-Host "   - HTTP:  http://localhost:5288" -ForegroundColor Green
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Sistema iniciado correctamente       " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Credenciales de prueba:" -ForegroundColor Yellow
Write-Host "  Usuario:    admin" -ForegroundColor White
Write-Host "  Contraseña: admin123" -ForegroundColor White
Write-Host ""

Write-Host "Presiona cualquier tecla para abrir el navegador..." -ForegroundColor Cyan
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Abrir el navegador
Start-Process "https://localhost:7276"

Write-Host ""
Write-Host "Para detener los servicios, cierra las ventanas de PowerShell correspondientes" -ForegroundColor Yellow
Write-Host "Presiona cualquier tecla para salir..." -ForegroundColor Cyan
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
