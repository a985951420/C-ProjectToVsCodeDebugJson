@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - Install Script
REM This script will package and install the tool globally

echo ========================================
echo VSCode Debug Generator Install Script
echo ========================================
echo.

REM Check if dotnet is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK not detected
    echo Please install .NET SDK 8.0 or higher first
    echo Download: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo [1/4] Cleaning old build files...
dotnet clean -c Release >nul 2>&1

echo [2/4] Packaging project...
dotnet pack -c Release -o ./nupkg
if errorlevel 1 (
    echo ERROR: Packaging failed
    pause
    exit /b 1
)

echo [3/4] Uninstalling old version (if exists)...
dotnet tool uninstall --global VsCodeDebugGen >nul 2>&1

echo [4/4] Installing tool globally...
dotnet tool install --global --add-source ./nupkg VsCodeDebugGen
if errorlevel 1 (
    echo ERROR: Installation failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Installation complete!
echo ========================================
echo.
echo You can now use the following commands anywhere:
echo   vscodegen --help
echo   vscodegen
echo.
pause
