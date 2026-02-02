@echo off
chcp 65001 >nul
REM VSCode Debug Generator - 安装脚本
REM 此脚本将打包并安装工具到全局环境

echo ========================================
echo VSCode Debug Generator 安装脚本
echo ========================================
echo.

REM 检查 dotnet 是否安装
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo 错误: 未检测到 .NET SDK
    echo 请先安装 .NET SDK 8.0 或更高版本
    echo 下载地址: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo [1/4] 清理旧的构建文件...
dotnet clean -c Release >nul 2>&1

echo [2/4] 打包项目...
dotnet pack -c Release -o ./nupkg
if errorlevel 1 (
    echo 错误: 打包失败
    pause
    exit /b 1
)

echo [3/4] 卸载旧版本（如果存在）...
dotnet tool uninstall --global VsCodeDebugGen >nul 2>&1

echo [4/4] 安装工具到全局环境...
dotnet tool install --global --add-source ./nupkg VsCodeDebugGen
if errorlevel 1 (
    echo 错误: 安装失败
    pause
    exit /b 1
)

echo.
echo ========================================
echo ✓ 安装完成！
echo ========================================
echo.
echo 现在你可以在任何地方使用以下命令:
echo   vscodegen --help
echo   vscodegen
echo.
pause
