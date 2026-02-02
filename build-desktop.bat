@echo off
chcp 65001 >nul
REM VSCode Debug Generator - 桌面应用构建脚本
REM 快速构建桌面应用（不打包，用于测试）

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - 桌面应用构建
echo ========================================
echo.

REM 检查 .NET 是否安装
where dotnet >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo [错误] 未找到 .NET SDK，请先安装 .NET 8.0 或更高版本
    echo 下载地址: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

REM 设置项目路径
set DESKTOP_PROJECT=%~dp0src\VsCodeDebugGen.Desktop\VsCodeDebugGen.Desktop.csproj

REM 检查项目文件是否存在
if not exist "%DESKTOP_PROJECT%" (
    echo [错误] 未找到桌面应用项目文件: %DESKTOP_PROJECT%
    pause
    exit /b 1
)

echo [信息] 正在构建桌面应用...
echo.

REM 构建项目
dotnet build "%DESKTOP_PROJECT%" --configuration Release

if %ERRORLEVEL% neq 0 (
    echo.
    echo [错误] 桌面应用构建失败
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo ========================================
echo [完成] 桌面应用构建成功！
echo ========================================
echo.
echo 输出位置: src\VsCodeDebugGen.Desktop\bin\Release\net8.0\
echo.
pause
