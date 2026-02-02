@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - CLI Launcher
REM Run CLI Command-line Tool

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - CLI Tool
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
set CLI_PROJECT=%~dp0src\VsCodeDebugGen.CLI\生成调试.csproj

REM Check if project file exists
if not exist "%CLI_PROJECT%" (
    echo [ERROR] CLI project file not found: %CLI_PROJECT%
    pause
    exit /b 1
)

echo [INFO] Starting CLI tool...
echo.

REM Run CLI tool with all command-line arguments
dotnet run --project "%CLI_PROJECT%" -- %*

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] CLI tool execution failed
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [DONE] CLI tool executed successfully
pause
