@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - Desktop Application Build Script
REM Quick build desktop application (without packaging, for testing)

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - Build Desktop
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

echo [INFO] Building desktop application...
echo.

REM Build project
dotnet build "%DESKTOP_PROJECT%" --configuration Release

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Desktop application build failed
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo ========================================
echo [DONE] Desktop application built successfully!
echo ========================================
echo.
echo Output location: src\VsCodeDebugGen.Desktop\bin\Release\net8.0\
echo.
pause
