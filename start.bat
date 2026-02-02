@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - Main Launcher
REM Choose CLI or Desktop Application

setlocal enabledelayedexpansion

:menu
cls
echo ========================================
echo  VSCode Debug Generator
echo  C# Project Debug Configuration Generator
echo ========================================
echo.
echo Please select launch mode:
echo.
echo  [1] CLI Command-line Tool
echo  [2] Desktop Application (GUI)
echo  [3] Build Desktop Application
echo  [4] Package Desktop Application (Standalone Executable)
echo  [0] Exit
echo.
echo ========================================
set /p choice=Enter option (0-4):

if "%choice%"=="1" goto run_cli
if "%choice%"=="2" goto run_desktop
if "%choice%"=="3" goto build_desktop
if "%choice%"=="4" goto publish_desktop
if "%choice%"=="0" goto exit
echo.
echo [ERROR] Invalid option, please try again
timeout /t 2 >nul
goto menu

:run_cli
cls
echo Starting CLI Command-line Tool...
echo.
call "%~dp0run-cli.bat"
goto menu

:run_desktop
cls
echo Starting Desktop Application...
echo.
call "%~dp0run-desktop.bat"
goto menu

:build_desktop
cls
echo Building Desktop Application...
echo.
call "%~dp0build-desktop.bat"
goto menu

:publish_desktop
cls
echo Packaging Desktop Application...
echo.
call "%~dp0publish-desktop.bat"
goto menu

:exit
echo.
echo Thank you for using VSCode Debug Generator!
echo.
exit /b 0
