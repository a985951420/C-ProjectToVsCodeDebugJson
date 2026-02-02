@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - Desktop Application Launcher
REM Run Avalonia Desktop Application (Development Mode)

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - Desktop App
echo ========================================
echo.

REM Check if .NET is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo [ERROR] .NET SDK not found, please install .NET 8.0 or higher
    echo Download: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

REM Set project path
set DESKTOP_PROJECT=%~dp0src\VsCodeDebugGen.Desktop\VsCodeDebugGen.Desktop.csproj

REM Check if project file exists
if not exist "%DESKTOP_PROJECT%" (
    echo [ERROR] Desktop project file not found: %DESKTOP_PROJECT%
    pause
    exit /b 1
)

echo [INFO] Starting desktop application...
echo [TIP] First run may take time to restore NuGet packages, please wait...
echo.

REM Run desktop application
dotnet run --project "%DESKTOP_PROJECT%"

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Desktop application failed to start
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [DONE] Desktop application closed
pause
