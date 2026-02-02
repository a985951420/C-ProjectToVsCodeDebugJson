@echo off
chcp 65001 >nul
REM VSCode Debug Generator - CLI 命令行启动脚本
REM 用于运行 CLI 命令行工具

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - CLI 工具
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
set CLI_PROJECT=%~dp0src\VsCodeDebugGen.CLI\生成调试.csproj

REM 检查项目文件是否存在
if not exist "%CLI_PROJECT%" (
    echo [错误] 未找到 CLI 项目文件: %CLI_PROJECT%
    pause
    exit /b 1
)

echo [信息] 正在启动 CLI 工具...
echo.

REM 运行 CLI 工具，传递所有命令行参数
dotnet run --project "%CLI_PROJECT%" -- %*

if %ERRORLEVEL% neq 0 (
    echo.
    echo [错误] CLI 工具执行失败
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [完成] CLI 工具执行成功
pause
