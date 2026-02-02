@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - Uninstall Script

echo ========================================
echo VSCode Debug Generator Uninstall Script
echo ========================================
echo.

echo Uninstalling global tool...
dotnet tool uninstall --global VsCodeDebugGen

if errorlevel 1 (
    echo ERROR: Uninstallation failed or tool not installed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Uninstallation complete!
echo ========================================
echo.
pause
