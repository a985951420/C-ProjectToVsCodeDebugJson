@echo off
chcp 65001 >nul 2>&1
REM VSCode Debug Generator - Desktop Application Package Script
REM Publish desktop application as standalone executable

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - Package Desktop
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
set OUTPUT_DIR=%~dp0publish\desktop

REM Check if project file exists
if not exist "%DESKTOP_PROJECT%" (
    echo [ERROR] Desktop project file not found: %DESKTOP_PROJECT%
    pause
    exit /b 1
)

echo [INFO] Publish configuration:
echo   - Project: VsCodeDebugGen.Desktop
echo   - Output directory: %OUTPUT_DIR%
echo   - Runtime: win-x64
echo   - Self-contained: Yes (includes .NET runtime)
echo   - Single file: Yes
echo.

REM Clean old publish files
if exist "%OUTPUT_DIR%" (
    echo [INFO] Cleaning old publish files...
    rmdir /s /q "%OUTPUT_DIR%"
)

echo [INFO] Starting to publish desktop application...
echo [TIP] This may take a few minutes, please wait...
echo.

REM Publish as single executable file (self-contained runtime)
dotnet publish "%DESKTOP_PROJECT%" --configuration Release --runtime win-x64 --self-contained true --output "%OUTPUT_DIR%" /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true /p:DebugType=None /p:DebugSymbols=false

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Desktop application publish failed
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo ========================================
echo [DONE] Desktop application packaged successfully!
echo ========================================
echo.
echo Output location: %OUTPUT_DIR%
echo Executable file: VsCodeDebugGen.exe
echo.
echo You can copy the entire publish\desktop folder to any Windows PC and run it
echo (no need to install .NET runtime)
echo.

REM Open output directory
echo [TIP] Press any key to open output directory...
pause >nul
explorer "%OUTPUT_DIR%"
